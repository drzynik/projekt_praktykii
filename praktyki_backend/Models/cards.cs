using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Cards
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cards_Id { get; set; }
        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }
        public int Card_Id { get; set; }
    }
}
