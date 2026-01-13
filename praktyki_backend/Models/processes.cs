using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Processes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Processes_Id { get; set; }
        public string Processes_Desc { get; set; }
        public string Processes_Desc_Long { get; set; }
        public string Processes_Color { get; set; }
        public double Processes_Weight { get; set; }

        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }
    }
}
