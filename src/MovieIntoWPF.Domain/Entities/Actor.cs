using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class Actor : Auditable
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Hobby { get; set; } = String.Empty;
        public bool Gender { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
