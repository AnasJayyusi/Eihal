using Eihal.Enums;

namespace Eihal.Models
{
    public class SupportServiceModal
    {
        public int ServiceId { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string Fee { get; set; }
        public string Price { get; set; }
        public ServicesStatusEnum Status { get; set; }

    }
}
