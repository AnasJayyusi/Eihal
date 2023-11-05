using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class TimeClinicLocation
    {
        public int Id { get; set; }

        [ForeignKey(nameof(UserProfile))]
        public int UserProfileId { get; set; }
        public string? ClinicName { get; set; }

        public string? SundayOpenAt { get; set; }
        public string? SundayClosedAt { get; set; }
        public bool SundayIsClosed { get; set; }

        public string? MondayOpenAt { get; set; }
        public string? MondayClosedAt { get; set; }
        public bool MondayIsClosed { get; set; }

        public string? TuesdayOpenAt { get; set; }
        public string? TuesdayClosedAt { get; set; }
        public bool TuesdayIsClosed { get; set; }

        public string? WednesdayOpenAt { get; set; }
        public string? WednesdayClosedAt { get; set; }
        public bool WednesdayIsClosed { get; set; }

        public string? ThursdayOpenAt { get; set; }
        public string? ThursdayClosedAt { get; set; }
        public bool ThursdayIsClosed { get; set; }

        public string? FridayOpenAt { get; set; }
        public string? FridayClosedAt { get; set; }
        public bool FridayIsClosed { get; set; }

        public string? SaturdayOpenAt { get; set; }
        public string? SaturdayClosedAt { get; set; }
        public bool SaturdayIsClosed { get; set; }

        #region DropDownList
        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }

        [ForeignKey(nameof(State))]
        public int? StateId { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }
        [ForeignKey(nameof(District))]
        public int? DistrictId { get; set; }
        #endregion

        #region Navigations 
        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
        public District? Districts { get; set; }
        public UserProfile UserProfile { get; set; }
        #endregion

        [NotMapped]
        public string Location { get; set; }
    }
}
