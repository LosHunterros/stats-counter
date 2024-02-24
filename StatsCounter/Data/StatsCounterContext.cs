using Microsoft.EntityFrameworkCore;
using StatsCounter.Models;

namespace StatsCounter.Data;
public class StatsCounterContext : DbContext
{
    //        public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<RepositoryStatsHistory> RepositoryStatsHistories { get; set; }

    public StatsCounterContext(DbContextOptions<StatsCounterContext> options) : base(options)
    {
    }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
