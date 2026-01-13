using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Hardwares
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Hardwares_Id { get; set; }
        [ForeignKey(nameof(Cards))]
        public int Cards_Id { get; set; }
        public Cards Cards { get; set; }
        public string Hardwares_Short_Desc { get; set; }
        public string Hardwares_Long_Desc { get; set; }
        public double Hardwares_Cost_Bits { get; set; }
        public int Hardwares_Cost_Bits_Weight { get; set; }
        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }

    }
}
