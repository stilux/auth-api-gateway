using AuthServer.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.DAL.Context
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userModelBuilder = builder.Entity<ApplicationUser>();
            userModelBuilder.Property(u => u.FamilyName).IsRequired();
            userModelBuilder.Property(u => u.GivenName).IsRequired();
        }
    }
}
