using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
  public DbSet<Product> Products { get; set; }
  public DbSet<Cart> Carts { get; set; }
}
