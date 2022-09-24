using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Configurations
{
    public class PaginationParams
    {
        public Int32 PageIndex { get; set; }
        public Int32 PageSize { get; set; }
        public int SkipCount { get
            {
                return PageSize * (PageIndex - 1);
            }
        }
    }
}
