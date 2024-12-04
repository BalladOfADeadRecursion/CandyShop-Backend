using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("cart")]
    public class Cart
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("clientid")]
        public long ClientId { get; set; }

        public Client? Client { get; set; }

        [JsonIgnore]
        public ICollection<CartItem> CartItems { get; set; }
    }
}
