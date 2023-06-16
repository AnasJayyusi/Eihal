using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class UserProfile
    {

        public int Id { get; set; }
        #region  Synced With AspNetUser
        public string? UserId { get; set; }
        public string? FullName { get; set; }
        public int? AccountTypeId { get; set; }
        public int? PractitionerTypeId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        #endregion

        #region DropDownList
        [ForeignKey(nameof(ProfessionalRank))]
        public int? ProfessionalRankId { get; set; }

        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }

        [ForeignKey(nameof(State))]
        public int? StateId { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }
        #endregion

        #region Editable Fields
        public string? ProfilePicturePath { get; set; }

        [MaxLength(4096)]
        public string? Bio { get; set; }
        #endregion

        #region Base On Login & Bussines
        public int? NumOfPatients { get; set; }
        public int? Reviews { get; set; }
        public bool? InsuranceAccepted { get; set; }
        #endregion

        #region Navigations 
        public ProfessionalRank? ProfessionalRank { get; set; }
        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
        public List<Certification> Certifications { get; set; }
        #endregion

        #region MultiSelection
        public string? SpecialtiesIds { get; set; }
        public string? SpecialtiesTitlesAr { get; set; }
        public string? SpecialtiesTitlesEn { get; set; }
        #endregion

        public ProfileStatus ProfileStatus { get; set; }


    }
}

public enum ProfileStatus
{
    UnCompleted = 0,
    UnderReview = 1,
    Rejected = 2,
    Active = 3
}