using DaycareManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DaycareManager.Data;

public class DaycareDbContext : DbContext
{
    public DaycareDbContext(DbContextOptions<DaycareDbContext> options) : base(options)
    {
    }

    public DbSet<Child> Children { get; set; }
    public DbSet<Parent> Parents { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: Seed data or further configuration
    }
}
