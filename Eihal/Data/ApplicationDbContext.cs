using Eihal.Data.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>().HasQueryFilter(f => f.AccountTypeId != 0); // Ignore Admin User From All Queries
            modelBuilder.Entity<UserServices>().HasQueryFilter(f => f.UserId != 0); // Ignore Admin User From All Queries
            modelBuilder.Entity<UserCompany>().HasKey(sc => new { sc.UserProfileId, sc.InsuranceCompanyId });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Services> Services { get; set; }
        public DbSet<UserServices> UserServices { get; set; }
        public DbSet<GeneralSettings> GeneralSettings { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PractitionerType> PractitionerTypes { get; set; }
        public DbSet<ProfessionalRank> ProfessionalRanks { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ReferralRequest> ReferralRequests { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Subspecialty> Subspecialty { get; set; }
        public DbSet<ClinicalSpeciality> ClinicalSpecialities { get; set; }
        public DbSet<Privillage> Privillages { get; set; }
        public DbSet<SubPrivillage> SubPrivillages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<InsuranceCompany> InsuranceCompanies { get; set; }
        public DbSet<TimeClinicLocation> TimeClinicLocations { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}