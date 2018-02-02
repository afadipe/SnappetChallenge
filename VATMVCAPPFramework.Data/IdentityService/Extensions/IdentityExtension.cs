
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace VATMVCAPPFramework.Data.IdentityService.Extensions
{
    public static class IdentityExtension
    {
        public static string GetFullName(this IIdentity identity)
        {

            var claim = ((ClaimsIdentity)identity);
            var fullname = claim.FindFirstValue("FullName");
            // Test for null to avoid issues during local testing
            return fullname ?? string.Empty;
        }
    }
}
