using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                UserName = "myuser",
                NormalizedUserName = "MYUSER",
                PasswordHash = hasher.HashPassword(null, "password")
            });




            base.OnModelCreating(builder);
        }

    }
}
