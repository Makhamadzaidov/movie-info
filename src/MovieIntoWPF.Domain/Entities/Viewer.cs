using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class Viewer : BaseEntity
    {
        public string Name { get; set; } = String.Empty;
    }
}
