using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace llm.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? OfficeLocation { get; set; }
        public string? MobilePhone { get; set; }
        public string? CompanyName { get; set; }
        public string? UserId { get; set; }
        public string? TenantId { get; set; }
        public string? AuthTime { get; set; }

        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var claims = User.Claims;
                
                DisplayName = GetClaimValue("name") ?? GetClaimValue("preferred_username");
                Email = GetClaimValue("preferred_username") ?? GetClaimValue("email");
                JobTitle = GetClaimValue("jobTitle");
                Department = GetClaimValue("department");
                OfficeLocation = GetClaimValue("officeLocation");
                MobilePhone = GetClaimValue("mobilePhone");
                CompanyName = GetClaimValue("companyName");
                UserId = GetClaimValue("oid") ?? GetClaimValue("sub");
                TenantId = GetClaimValue("tid");
                
                var authTimeValue = GetClaimValue("auth_time");
                if (!string.IsNullOrEmpty(authTimeValue) && long.TryParse(authTimeValue, out var authTimestamp))
                {
                    AuthTime = DateTimeOffset.FromUnixTimeSeconds(authTimestamp).ToString("yyyy-MM-dd HH:mm:ss UTC");
                }
                else
                {
                    AuthTime = "Not available";
                }
            }
        }

        private string? GetClaimValue(string claimType)
        {
            return User.Claims?.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
