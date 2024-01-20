using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class RequiredAttachment
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        // Use This Later To Avoid Duplicate Certification In UI & Delete Issues
        public string? Name { get; set; }
        public string? Path { get; set; }
        public RequiredFileType RequiredFileType { get; set; }
        public string? Extension { get; set; }
        [ForeignKey(nameof(UserProfile))]
        public int? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }

    }

    public enum RequiredFileType
    {
        SignedContract = 1,
        ProfessionalCategory = 2
    }
}
