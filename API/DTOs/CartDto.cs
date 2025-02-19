using System;

namespace API.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public required string CartId { get; set; }
    public List<CartItemDto> Items { get; set; } = [];
}
