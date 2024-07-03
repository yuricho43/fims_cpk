using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FimsCPK.Entities;

public class AspNetUser : IdentityUser
{
    public string? HangulName { get; set; }

    public string? EnglishName { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
