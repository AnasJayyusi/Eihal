namespace Eihal.Data.Entites
{
    public class UserCompany
    {
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int InsuranceCompanyId { get; set; }
        public InsuranceCompany Company { get; set; }
    }
}
