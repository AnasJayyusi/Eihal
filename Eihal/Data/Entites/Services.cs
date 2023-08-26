using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class Services
    {
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public bool IsActive { get; set; }
        public double Fee { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        //[ForeignKey(nameof(Privillage))]
        //public int? PrivillageId { get; set; }
        [ForeignKey(nameof(SubPrivillage))]
        public int? SubPrivillageId { get; set; }
        //public Privillage? Privillage { get; set; }
        public SubPrivillage? SubPrivillage { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        [ForeignKey(nameof(ClinicalSpeciality))]
        public int? ClinicalSpecialityId { get; set; }
        public ClinicalSpeciality? ClinicalSpeciality { get; set; }

    }
}
