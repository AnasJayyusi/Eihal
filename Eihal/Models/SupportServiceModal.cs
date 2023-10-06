using Eihal.Enums;

namespace Eihal.Models
{
    public class SupportServiceModal
    {
        public int UserProfileId { get; set; }
        public int ServiceId { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string ClinicalSpecialityNameAr { get; set; }
        public string ClinicalSpecialityNameEn { get; set; }
        public string Logo { get; set; }
        public ServicesStatusEnum Status { get; set; }
        public List<string> ClinicalSpecialties { get; set; }
    }
}
