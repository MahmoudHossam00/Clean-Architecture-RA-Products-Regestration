namespace RAProductsRegestration.Infrastructure.Integrations.CTD.Models
{
    internal sealed class CTDProductRow
    {
        public int Id { get; set; }

        public string ProductCode { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string CompanyCode { get; set; } = null!;
    }
}
