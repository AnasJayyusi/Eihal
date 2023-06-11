using System.ComponentModel.DataAnnotations.Schema;

namespace Eihal.Data.Entites
{
    public class State
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }

        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }
        public Country Country { get; set; }

        public List<City> Cities { get; set; }
    }
}
