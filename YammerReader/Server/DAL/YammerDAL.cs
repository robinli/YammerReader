using Dapper;
using System.Data;
using System.Data.Common;
using YammerReader.Shared;

namespace YammerReader.Server.DAL
{
    public class YammerDAL : BaseDAL
    {
        private string GetSqlforPaging(string selectSql, string orderbySql, QueryBase query)
        {
            if (!query.IsPaging)
            {
                return selectSql;
            }

            return @$"WITH P AS(
{selectSql}
)
select P.*, ttlrows=convert(int, COUNT(1) OVER ())
from P
order by {orderbySql}
OFFSET {(query.PageIndex - 1) * query.PageSize} ROWS
FETCH NEXT {query.PageSize} ROWS ONLY";
        }

        public async Task<YammerGroup?> GetGroup(string group_id)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, group_name, order_num
from dbo.Groups
where id = @group_id";
            var parms = new DynamicParameters();
            parms.Add("group_id", group_id);
            YammerGroup? data = (await connection.QueryAsync<YammerGroup>(tsql, parms))?.FirstOrDefault();

            return data;
        }

        public async Task<List<YammerGroup>> GetAllGroups()
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, group_name, order_num
from dbo.Groups
order by order_num";
            List<YammerGroup> data = (await connection.QueryAsync<YammerGroup>(tsql)).ToList();

            return data;
        }


        public async Task<List<YammerMessage>> GetGroupThreads(YammerFilter filter)
        {
            string group_id = filter!.group_id!;

            DbConnection connection = GetSqlConnection();
            string selectSql = $@"select id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at, thread_count, thread_last_at 
from dbo.viewMessages M
where M.group_id = @group_id
and M.parent_id = ''";
            string orderbySql = "thread_last_at desc";

            string tsql = GetSqlforPaging(selectSql, orderbySql, filter);

            var parms = new DynamicParameters();
            parms.Add("group_id", group_id, dbType: DbType.String, size: 16);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            await GetMessageAttachments(data);

            await GetLastReplies(data);

            return data;
        }

        /// <summary>
        /// 讀取每一則討論串最後兩筆回覆
        /// </summary>
        /// <param name="thread_id"></param>
        /// <returns></returns>
        private async Task GetLastReplies(List<YammerMessage> data)
        {
            if (data == null || data.Any() == false)
            {
                return;
            }

            DbConnection connection = GetSqlConnection();
            string tsql = $@"select top 2 id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
from dbo.viewMessages M
where M.thread_id = @thread_id
and M.parent_id<>''
order by created_at desc";

            foreach(YammerMessage message in data)
            {
                //var parms = new DynamicParameters();
                //parms.Add("thread_id", message.thread_id);
                List<YammerMessage> replyMessage = (await connection.QueryAsync<YammerMessage>(tsql
                    , new { thread_id = message.thread_id }
                    )).ToList();

                if (replyMessage.Any() == false)
                {
                    continue;
                }

                await GetMessageAttachments(replyMessage);

                message.Replies = new List<YammerMessage>();
                foreach (YammerMessage item in replyMessage)
                {
                    message.Replies.Add(item);
                }
            }

            return ;
        }

        public async Task<List<YammerMessage>> GetThreadReplies(string thread_id, int offset_rows)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
from dbo.viewMessages M
where M.thread_id = @thread_id
and M.parent_id<>''
order by created_at desc
OFFSET @offset_rows ROWS";
            var parms = new DynamicParameters();
            parms.Add("thread_id", thread_id);
            parms.Add("offset_rows", offset_rows);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            await GetMessageAttachments(data);

            return data;
        }

        public async Task<YammerMessage?> SingleThread(string thread_id)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
, thread_count
, thread_last_at
from dbo.viewMessages M
where M.thread_id = @thread_id
order by created_at asc";
            var parms = new DynamicParameters();
            parms.Add("thread_id", thread_id);

            var queryResult = (await connection.QueryAsync<YammerMessage>(tsql, parms));
            if (queryResult==null || queryResult.Any() == false)
            {
                return null;
            }
            List<YammerMessage> data= queryResult.ToList();
            await GetMessageAttachments(data);
            YammerMessage returnThread = data![0];
            returnThread.Replies = new List<YammerMessage>();
            for (int i = 1; i < data.Count; i++)
            {
                returnThread.Replies.Add(data[i]);
            }
            return returnThread;
        }

        public async Task<List<YammerMessage>> Search(YammerFilter filter)
        {
            string pure_keyword = filter!.search_keyword!;
            string search_keyword = $"%{pure_keyword}%";
            
            DbConnection connection = GetSqlConnection();

            /*查詢 第一階 資料*/
            string selectSql = $@"select id, replied_to_id, parent_id, M.thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at, thread_count, thread_last_at
, P1.match_rows
from dbo.viewMessages M
inner join(
	select thread_id, match_rows=count(1)
	from dbo.Messages m0
	where m0.body like @search_keyword
	group by thread_id
) P1
	on P1.thread_id=M.id";

            string orderbySql = "thread_last_at desc";

            string tsql = GetSqlforPaging(selectSql, orderbySql, filter);

            var parms = new DynamicParameters();
            parms.Add("search_keyword", search_keyword);
            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();

            foreach(YammerMessage item in data)
            {
                //強調關鍵字
                item.body = item.body.Replace(pure_keyword, $"<em>{pure_keyword}</em>");

                /*查詢 回覆 資料*/
                if (item.match_rows == 1)
                {
                    continue;
                }
                
                string selectReplySql = @"select id, replied_to_id, parent_id, thread_id, thread_line_no
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
from dbo.Messages m0
where thread_id = @thread_id
and m0.body like @search_keyword
and parent_id<>''";
                
                var replyParms = new DynamicParameters();
                replyParms.Add("thread_id", item.thread_id);
                replyParms.Add("search_keyword", search_keyword);
                List<YammerMessage> replyData = (await connection.QueryAsync<YammerMessage>(selectReplySql, replyParms)).ToList();
                foreach(YammerMessage reply in replyData)
                {
                    if (item.Replies == null)
                    {
                        item.Replies = new List<YammerMessage>();
                    }
                    //High light 關鍵字
                    reply.body = reply.body.Replace(pure_keyword, $"<em>{pure_keyword}</em>");
                    item.Replies.Add(reply);
                }
            }
            //await GetMessageAttachments(data);
            return data;
        }

        private async Task GetMessageAttachments(List<YammerMessage>? messages)
        {
            await Task.Delay(0);
            if(messages == null || messages.Any()==false)
            {
                return;
            }
            //將 id 串接成一個字串
            var rarray = (from m in messages select m.id);
            string message_id = string.Join(",", rarray);

            DbConnection connection = GetSqlConnection();
            var procedure = "[dbo].[GetMessageAttachments]";
            var parms = new DynamicParameters();
            parms.Add("message_id", message_id);
            var attachments = (await connection.QueryAsync<YammerFile>(procedure, parms, commandType: CommandType.StoredProcedure));
            if(attachments.Any() == false)
            {
                return;
            }
            foreach (YammerFile file in attachments)
            {
                YammerMessage? msg = messages.FirstOrDefault(m => m.id.Equals(file.message_id));
                if (msg == null)
                {
                    continue;
                }
                if (msg.AttachmentFiles == null)
                {
                    msg.AttachmentFiles = new List<YammerFile>();
                }
                msg.AttachmentFiles.Add(file);
            }
            return;
        }
    }
}
