using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Decks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Decks_Id { get; set; }
        public string Deck_Name { get; set; }

        [ForeignKey(nameof(Users))]
        public int Users_Id { get; set; }
        public Users Users { get; set; }
    }
}
