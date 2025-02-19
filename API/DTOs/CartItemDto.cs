using System;

namespace API.DTOs;

public class CartItemDto
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public long Price { get; set; } 
    public required string PictureUrl { get; set; }  
    public required string Brand { get; set; }
    public required string Type { get; set; }
    public int Quantity { get; set; }

}
