namespace Eihal.Data.Entites
{
    public class InsuranceCompany
    {
        public int Id { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Base64Image { get; set; }
        public bool IsActive { get; set; }
    }
}
