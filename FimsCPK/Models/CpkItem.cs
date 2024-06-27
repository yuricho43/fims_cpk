using System;
using System.Collections.Generic;

namespace FimsCPK.Models;

public partial class CpkItem
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public int TestNo { get; set; }

    public string? Ch1Lcl { get; set; }

    public string? Ch1Ucl { get; set; }

    public string? Ch2Lcl { get; set; }

    public string? Ch2Ucl { get; set; }

    public string? Ch3Lcl { get; set; }

    public string? Ch3Ucl { get; set; }

    public string? Ch4Lcl { get; set; }

    public string? Ch4Ucl { get; set; }

    public string? Reserved1 { get; set; }

    public string? Reserved2 { get; set; }
}
