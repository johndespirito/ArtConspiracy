using ArtConspiracy.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArtConspiracy
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext()
            : base("DefaultConnection")
        {
        }
    }
}