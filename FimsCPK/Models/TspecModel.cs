using System;
using System.Collections.Generic;

namespace FimsCPK.Models;

public partial class TspecModel
{
    public int Id { get; set; }

    public string? ProductModel { get; set; }

    public int? NumChannel { get; set; }

    public string? CreatorName { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
