using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class YammerMessage
    {
        /// <summary>
        /// 回覆的訊息
        /// </summary>
        public List<YammerMessage>? Replies { get; set; }

        /// <summary>
        /// 附加檔案
        /// </summary>
        public List<YammerFile>? AttachmentFiles { get; set; }


        public string id { get; set; } = null!;
        public string replied_to_id { get; set; } = null!;
        public string parent_id { get; set; } = null!;
        public string thread_id { get; set; } = null!;
        public int thread_line_no { get; set; }
        public string group_id { get; set; } = null!;
        public string group_name { get; set; } = null!;
        public string sender_id { get; set; } = null!; 
        public string sender_name { get; set; } = null!;
        public string body { get; set; } = null!;
        public string attachments { get; set; } = null!;
        public DateTime created_at { get; set; }
        public int thread_count { get; set; }
        public DateTime thread_last_at { get; set; }

        public int ttlrows { get; set; }

        /// <summary>
        /// 討論串中符合 關鍵字 的筆數
        /// </summary>
        public int match_rows { get; set; }

        public int CountPrevoiusReplies()
        {
            if (parent_id != "")
            {
                return 0;
            }

            int replyCount = 0;
            if (Replies != null && Replies.Any())
            {
                replyCount = Replies.Count();
            }
            return thread_count - replyCount;
        }
    }
}
