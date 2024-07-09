using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tickets.Domain.Entities
{
    public class User : IdentityUser
    {
        public IList<Task>? Tasks { get; set; } = new List<Task>();
    }
}
