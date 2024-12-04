using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("Order")]
    public class Order
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("clientid")]
        public long ClientId { get; set; }

        [Column("orderdate")]
        public DateOnly OrderDate { get; set; }

        [Column("totalamount")]
        public int TotalAmount { get; set; }

        /*[Column("pickuppointid")]
        public long PickupPointId { get; set; }*/

        public Client? Client { get; set; }

        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
