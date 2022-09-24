using MovieInto.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieInto.Domain.Entities
{
    public class Director : Auditable
    {
        public String FirstName { get; set; } = String.Empty;
        public String LastName { get; set; } = String.Empty;
        public bool Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public String Position { get; set; } = String.Empty;
        public String Hobby { get; set; } = String.Empty;
    }
}
