namespace Eihal.Data.Entites
{
    public class PractitionerType
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public List<Specialty> Specialties { get; set; }
    }
}
