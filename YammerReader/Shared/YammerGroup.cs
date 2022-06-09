using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class YammerGroup
    {
        public string id { get; set; } = null!;
        public string group_name { get; set; } = null!;
        public int order_num { get; set; }
    }
}
