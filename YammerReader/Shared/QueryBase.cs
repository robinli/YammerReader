using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerReader.Shared
{
    public class QueryBase
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 1;

        public bool IsPaging { get { return PageSize > 1; } }
    }
}
