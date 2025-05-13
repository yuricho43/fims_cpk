using System.ComponentModel.DataAnnotations;

namespace FimsCPK.Data
{
    public class NewCpkRegisterRequestModel
    {
        public string ? ModelName { get; set; }

        [Required]
        public bool bForAllModel { get; set; }

        [Required]
        public int iTestNo { get; set; }

        public string? Name { get; set; }

        [Required]
        public string LCL1 { get; set; }
        [Required]
        public string UCL1 { get; set; }

        public string? LCL2 { get; set; }
        public string? UCL2 { get; set; }
        public string? LCL3 { get; set; }
        public string? UCL3 { get; set; }
        public string? LCL4 { get; set; }
        public string? UCL4 { get; set; }
    }
}
