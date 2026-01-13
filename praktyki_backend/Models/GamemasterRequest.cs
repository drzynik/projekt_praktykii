using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class GamemasterRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Request_Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
