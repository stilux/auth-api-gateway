using Microsoft.AspNetCore.Identity;

namespace AuthServer.DAL.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string GivenName { get; set; }
        
        public string FamilyName { get; set; }
    }
}
