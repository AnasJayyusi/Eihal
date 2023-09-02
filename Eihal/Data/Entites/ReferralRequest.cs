using Eihal.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class ReferralRequest
    {
        public int Id { get; set; }
        public ReferralStatusEnum Status { get; set; }
        public DateTime? CompletionDate { get; set; }
        public ReferralTypeEnum Type { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey(nameof(CreatedByUser))]
        public int CreatedByUserId { get; set; }
        public UserProfile CreatedByUser { get; set; }
        [ForeignKey(nameof(AssignedToUser))]
        public int AssignedToUserId { get; set; }
        public UserProfile AssignedToUser { get; set; }
        public OrderDetail Order { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }


    }
}
