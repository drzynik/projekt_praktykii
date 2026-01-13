using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Decisions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Decisions_Id { get; set; }
        [ForeignKey(nameof(Cards))]
        public int Cards_Id { get; set; }
        public Cards Cards { get; set; }
        public string Decisions_Short_Desc { get; set; }
        public string Decisions_Long_Desc { get; set; }
        public double Decisions_Cost_Bits { get; set; }
        public int Decisions_Cost_Bits_Weight { get; set; }
        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }
    }
}
