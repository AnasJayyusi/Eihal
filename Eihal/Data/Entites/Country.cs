namespace Eihal.Data.Entites
{
    public class Country
    {
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public List<State> States { get; set; }
    }
}
