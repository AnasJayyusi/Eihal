using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Certification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        // Use This Later To Avoid Duplicate Certification In UI & Delete Issues
        public int UniqueFileName { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Extension { get; set; }

        public string? UniversityNameAr { get; set; }
        public string? UniversityNameEn { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public int? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }

        [ForeignKey(nameof(Degree))]
        public int? DegreeId { get; set; }
        public Degree? Degree { get; set; }
    }
}
