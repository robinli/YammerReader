using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class YammerFile
    {
        public string message_id { get; set; } = null!;
        public int row_no { get; set; }
        public string file_name { get; set; } = null!;
        public string file_path { get; set; } = null!;
        public string file_type { get; set; } = null!;
        public string file_url { get; set; } = null!;
    }
}
