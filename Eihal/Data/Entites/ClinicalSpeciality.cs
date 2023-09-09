using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class ClinicalSpeciality
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(PractitionerType))]
        public int? PractitionerTypeId { get; set; }
        public string? LogoImagePath { get; set; }
        public PractitionerType PractitionerType { get; set; }

    }
}
