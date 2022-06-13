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
            string selectSql = $@"select id, replied_to_id, parent_id, thread_id
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at, thread_count, thread_last_at 
from dbo.viewMessages M
where M.group_id = @group_id
and M.parent_id = ''";
            string orderbySql = "thread_last_at desc";

            string tsql = GetSqlforPaging(selectSql, orderbySql, filter);

            var parms = new DynamicParameters();
            parms.Add("group_id", group_id);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            await GetMessageAttachments(data);
            
            return data;
        }


        public async Task<List<YammerMessage>> GetThreadReplies(string thread_id)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, replied_to_id, parent_id, thread_id
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
from dbo.viewMessages M
where M.thread_id = @thread_id
and M.parent_id<>''
order by thread_last_at asc";
            var parms = new DynamicParameters();
            parms.Add("thread_id", thread_id);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            await GetMessageAttachments(data);

            return data;
        }

        public async Task<YammerMessage> SingleThread(string thread_id)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select id, replied_to_id, parent_id, thread_id
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at
, thread_count = IIF(M.parent_id='', M.thread_count, 0)
, thread_last_at = IIF(M.parent_id='', M.thread_last_at, M.created_at)
from dbo.viewMessages M
where M.thread_id = @thread_id
order by created_at asc";
            var parms = new DynamicParameters();
            parms.Add("thread_id", thread_id);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            if (data.Any() == false)
            {
                return null;
            }
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
            string search_keyword = $"%{filter!.search_keyword!}%";

            DbConnection connection = GetSqlConnection();
            string selectSql = $@"select id, replied_to_id, parent_id, thread_id
, group_id, group_name, sender_id, sender_name
, body, attachments, created_at, thread_count, thread_last_at 
from dbo.viewMessages M
where M.body like @search_keyword
and M.parent_id = ''";
            string orderbySql = "thread_last_at desc";

            string tsql = GetSqlforPaging(selectSql, orderbySql, filter);

            var parms = new DynamicParameters();
            parms.Add("search_keyword", search_keyword);

            List<YammerMessage> data = (await connection.QueryAsync<YammerMessage>(tsql, parms)).ToList();
            await GetMessageAttachments(data);

            return data;
        }

        private async Task GetMessageAttachments(List<YammerMessage>? messages)
        {
            await Task.Delay(0);
            if(messages == null)
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
