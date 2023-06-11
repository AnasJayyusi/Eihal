using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Specialty
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(PractitionerType))]
        public int? PractitionerTypeId { get; set; }
        public PractitionerType PractitionerType { get; set; }

    }
}
