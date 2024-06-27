namespace FimsCPK.Models
{
    public class CpkItemDto
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string TestNo { get; set; } = null!;

        public string? Ch1Lcl { get; set; }

        public string? Ch1Ucl { get; set; }

        public string? Ch2Lcl { get; set; }

        public string? Ch2Ucl { get; set; }

        public string? Ch3Lcl { get; set; }

        public string? Ch3Ucl { get; set; }

        public string? Ch4Lcl { get; set; }

        public string? Ch4Ucl { get; set; }
    }
}
