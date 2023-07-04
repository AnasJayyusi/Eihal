using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Subspecialty
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(Specialty))]
        public int? SpecialityId { get; set; }
        public Specialty Specialty { get; set; }

    }
}
