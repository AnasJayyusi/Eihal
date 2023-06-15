using Eihal.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class UserServices
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; }
        public Services Service { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public ServicesStatusEnum Status { get; set; }
        public double Fee { get; set; }
        public double Price { get; set; }
        public string? RejectionReason { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
