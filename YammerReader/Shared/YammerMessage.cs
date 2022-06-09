﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class YammerMessage
    {
        public List<YammerMessage>? Replies { get; set; }

        public string id { get; set; } = null!;
        public string replied_to_id { get; set; } = null!;
        public string parent_id { get; set; } = null!;
        public string thread_id { get; set; } = null!;
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
    }
}
