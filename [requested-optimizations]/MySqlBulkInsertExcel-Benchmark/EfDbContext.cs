using Microsoft.EntityFrameworkCore;
namespace MySqlBulkInsertExcel_Benchmark;

public class EfDbContext(DbContextOptions<EfDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
}

public class User
{
    public long Id { get; set; }
    public int CityId { get; set; }
    public ICollection<Tag> Tags { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long UserId { get; set; }
    public DateTime? ExpireDate { get; set; }
    public User User { get; set; }
}