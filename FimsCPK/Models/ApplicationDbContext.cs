using FimsCPK.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FimsCPK.Models
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
        }

    }
}
