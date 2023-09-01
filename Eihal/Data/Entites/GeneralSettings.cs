using Eihal.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class GeneralSettings
    {
        public int Id { get; set; }
        public string VatValue { get; set; }
        public string? SitePercentage { get; set; }
        public string? IBAN { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
