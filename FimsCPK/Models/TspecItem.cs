using System;
using System.Collections.Generic;

namespace FimsCPK.Models;

public partial class TspecItem
{
    public int Id { get; set; }

    public string? Category { get; set; }

    public int TestNo { get; set; }

    public string? Title { get; set; }

    public string? Unit { get; set; }

    public int Channels { get; set; }

    public string? Applicable { get; set; }

    public string? ExpressionMode { get; set; }

    public string? Ch1Ucl { get; set; }

    public string? Ch1Lcl { get; set; }

    public string? Ch2Ucl { get; set; }

    public string? Ch2Lcl { get; set; }

    public string? Ch3Ucl { get; set; }

    public string? Ch3Lcl { get; set; }

    public string? Ch4Ucl { get; set; }

    public string? Ch4Lcl { get; set; }

    public string? Example { get; set; }

    public int TspecModelId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual TspecModel TspecModel { get; set; } = null!;
}
