using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("cartitem")]
    public class CartItem
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("cartid")]
        public long CartId { get; set; }

        [Column("candyid")]
        public long CandyId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public Cart? Cart { get; set; }
        public Candy? Candy { get; set; }
    }
}
