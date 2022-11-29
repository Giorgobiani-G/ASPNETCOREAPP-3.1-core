using Microsoft.AspNetCore.Identity;

namespace ASPNETCOREAPP.Models
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string Email) 
        { return new IdentityError { Code = nameof(DuplicateEmail), Description = "Aseti Meilit Registrirebuli momxmarebeli ukve arsebobs" }; }
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = "Aseti momxmarebeli ukve arsebobs" }; }
    }
}
