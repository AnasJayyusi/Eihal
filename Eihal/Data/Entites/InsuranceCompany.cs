namespace Eihal.Data.Entites
{
    public class InsuranceCompany
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? LogoImagePath { get; set; }
        public bool IsActive { get; set; }
        public List<UserProfile> UserProfiles { get; set; }
    }
}
