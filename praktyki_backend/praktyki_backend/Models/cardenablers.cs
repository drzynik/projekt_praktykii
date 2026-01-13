using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Cardenablers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cards_Enablers_Id { get; set; }
        [ForeignKey(nameof(Cards))]
        public int Cards_Id { get; set; }
        public Cards Cards { get; set; }
        public int Enablers_Id { get; set; }
    }
}
