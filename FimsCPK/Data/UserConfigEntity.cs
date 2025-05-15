using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FimsCPK.Data
{
    public class UserConfigEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ConfigName { get; set; }
        public string StateJson { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
