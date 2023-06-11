using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class ProfessionalRank
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }

        [ForeignKey(nameof(PractitionerType))]
        public int? PractitionerTypeId { get; set; }
        public PractitionerType PractitionerType { get; set; }

    }
}
