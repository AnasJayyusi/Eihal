﻿using Eihal.Data.Entites;
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

        public DbSet<Services> Services { get; set; }
        public DbSet<UserServices> UserServices { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PractitionerType> PractitionerTypes { get; set; }
        public DbSet<ProfessionalRank> ProfessionalRanks { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
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

    }
}