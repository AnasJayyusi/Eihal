using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class City
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }

        [ForeignKey(nameof(State))]
        public int? StateId { get; set; }
        public State State { get; set; }

        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
