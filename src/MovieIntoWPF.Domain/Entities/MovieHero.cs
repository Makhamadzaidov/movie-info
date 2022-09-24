using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class MovieHero : BaseEntity
    {
        public Int64 ActorId { get; set; }
        public Int64 MovieId { get; set; }
        public string HeroName { get; set; } = String.Empty;
    }
}
