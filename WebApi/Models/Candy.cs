using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("candy")]
    public class Candy
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("size")]
        public int Size { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("categoryid")]
        public long CategoryId { get; set; }

        public Category? Category { get; set; }

        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
