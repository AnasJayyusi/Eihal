using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class SubPrivillage
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(Privillage))]
        public int? PrivillageId { get; set; }
        public Privillage Privillage { get; set; }

    }
}
