using Eihal.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Eihal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

            // No Code Here 
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PractitionerType> PractitionerTypes { get; set; }
        public DbSet<ProfessionalRank> ProfessionalRank { get; set; }

    }
}