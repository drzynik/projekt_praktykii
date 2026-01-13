using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Gameevents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Games_Events_Id { get; set; }
        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }
        public string Events_Short_Desc { get; set; }
        public string Events_Long_Desc { get; set; }
        public int Turns_Time { get; set; }
        public double? Decisions_Cost_Bits_Weight { get; set; }
        public double? Hardware_Cost_Bits_Weight { get; set; }
        public double? Software_Cost_Bits_Weight { get; set; }
        public double? Boosters_X {  get; set; }
        public double? Boosters_Y { get; set; }
    }
}
