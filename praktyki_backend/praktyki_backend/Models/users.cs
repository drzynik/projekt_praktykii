using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace praktyki_backend.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Users_Id { get; set; }
        public string Names { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
