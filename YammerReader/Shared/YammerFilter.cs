using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class YammerFilter : QueryBase
    {
        public string? group_id { get; set; }

        public string? thread_id { get; set; }
    }
}
