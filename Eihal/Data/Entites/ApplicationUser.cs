using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class ApplicationUser : IdentityUser
    {

        public string? FullName { get; set; }

        [ForeignKey(nameof(AccountType))]
        public int? AccountTypeId { get; set; }
        public AccountType? AccountType { get; set; }

        [ForeignKey(nameof(PractitionerType))]
        public int? PractitionerTypeId { get; set; }
        public PractitionerType? PractitionerType { get; set; }

        [ForeignKey(nameof(ProfessionalRank))]
        public int? ProfessionalRankId { get; set; }
        public ProfessionalRank? ProfessionalRank { get; set; }
    }
}
