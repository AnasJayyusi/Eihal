using Eihal.Data.Entites;

namespace Eihal.Models
{
    public class DoctorDetailsModel
    {
        public int UserProfileId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public bool? InsuranceAccepted { get; set; }
        public OverView OverView { get; set; }
    }


}

public class OverView
{
    public string Bio { get; set; }
    public string? SpecialtiesTitlesAr { get; set; }
    public string? SpecialtiesTitlesEn { get; set; }
    public List<Certification> Certifications { get; set; }
}