using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public Int64 MovieId { get; set; }
        public Int64 ViewerId { get; set; }
        public string Comment { get; set; } = String.Empty;
        public float ViewerBall { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
