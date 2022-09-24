using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class Movie : Auditable
    {
        public string Name { get; set; } = String.Empty;
        public DateOnly MovieYear { get; set; }
        public string Duration { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
        public DateOnly PremiereDate { get; set; }
    }
}
