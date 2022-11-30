using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tryitter.Domain.Entity
{
    public class Student
    {
        public  Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
    }
}
