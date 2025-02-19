using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("CartItems")]
public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    
    // navigation props
    public int ProductId { get; set; }
    public required Product Product { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
}