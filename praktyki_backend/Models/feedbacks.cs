using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace praktyki_backend.Models
{
    public class Feedbacks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Feedbacks_Id { get; set; }
        [ForeignKey(nameof(Cards))]
        public int Cards_Id { get; set; }
        public Cards Cards { get; set; }
        public bool Status { set; get; }
        public string? Feedbacks_Long_Description { get; set; }
        [ForeignKey(nameof(Decks))]
        public int Decks_Id { get; set; }
        public Decks Decks { get; set; }
    }
}
