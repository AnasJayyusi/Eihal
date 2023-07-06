using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Privillage
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ClinicalSpeciality))]
        public int? ClinicalSpecialityId { get; set; }
        public ClinicalSpeciality ClinicalSpeciality { get; set; }

    }
}
