using Microsoft.AspNetCore.Identity;

namespace CarRental.Entities.Auth
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
