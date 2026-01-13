using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Cardweights
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cards_Weights_Id { get; set; }
        [ForeignKey(nameof(Cards))]
        public int Cards_Id { get; set; }
        public Cards Cards { get; set; }
        [ForeignKey(nameof(Processes))]
        public int Processes_Id { get; set; }
        public Processes Processes { get; set; }
        public int? Weights_X { get; set; }
        public int? Weights_Y { get; set; }

    }
}
