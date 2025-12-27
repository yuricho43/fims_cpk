using System;
using System.Collections.Generic;

namespace FimsCPK.Models;

public partial class CoolantPurchase
{
    public int Id { get; set; }

    public string CoolantName { get; set; } = null!;

    public double Amount { get; set; }

    public string? Etc { get; set; }

    public DateTime DtPurchased { get; set; }

    public DateTime? DtUpdated { get; set; }
}
