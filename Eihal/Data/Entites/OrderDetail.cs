using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Age Age { get; set; }
        public string ChronicDisease { get; set; }
        public List<Services> Services { get; set; }
        public int DoctorId { get; set; } // Releated with UserProfileId 

        #region DDL
        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }

        [ForeignKey(nameof(State))]
        public int? StateId { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }
        #endregion

        #region Navigations 
        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
        #endregion
    }
}

public enum Gender
{
    Female = 0,
    Male = 1
}

public enum Age
{
    Child = 0, // 4 - 10 
    Teenager = 1, // 11 -25
    Young = 2, // 26 - 50
    Aged = 3 // > 50 
}