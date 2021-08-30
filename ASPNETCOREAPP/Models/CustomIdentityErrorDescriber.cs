using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCOREAPP.Models
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string Email) 
        { return new IdentityError { Code = nameof(DuplicateEmail), Description = "Aseti Meilit Registrirebuli momxmarebeli ukve arsebobs" }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = "Aseti momxmarebeli ukve arsebobs" }; }
    }
}
