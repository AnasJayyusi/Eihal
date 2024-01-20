using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class OrderServiceDetail
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public double Price { get; set; }
        public double Fee { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        [ForeignKey(nameof(OrderDetail))]
        public int? OrderDetailId { get; set; }
        public int? Qty { get; set; }
        public bool IsCompleted { get; set; }
    }
}
