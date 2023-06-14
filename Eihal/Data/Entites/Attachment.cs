using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Attachment
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Extension { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public int? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
