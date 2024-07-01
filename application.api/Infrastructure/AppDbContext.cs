using Microsoft.EntityFrameworkCore;
using Domain.Entity;

namespace Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Test> Tests { get; set; }
    public DbSet<Officer> Officers { get; set; }
    public DbSet<InmateData> InmateDatas { get; set; }
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e=>e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name);
        });

        modelBuilder.Entity<Facility>().ToTable("Facility");
        modelBuilder.Entity<Officer>().ToTable("Officer");
        modelBuilder.Entity<InmateData>().ToTable("InmateData");
        modelBuilder.Entity<Transfer>().ToTable("Transfer");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "server=localhost;port=3306;user=root;password=romankarki;database=inmatesdb;";
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
