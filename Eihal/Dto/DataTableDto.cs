namespace Eihal.Dto
{
    public class DataTableDto
    {
        public string ServiceCode { get; set; }
        public string ServiceDesc { get; set; }
        public int Qty { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }
        public double VatPercentage { get; set; }
        public double VatValue { get; set; }
        public double NetWithVat { get; set; }
    }
}
