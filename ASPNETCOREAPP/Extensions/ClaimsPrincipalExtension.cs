using System.Linq;
using System.Security.Claims;
namespace MyProject.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var Name = principal.Claims.FirstOrDefault(c => c.Type == "Name");
            return Name?.Value;
        }
    }
}
