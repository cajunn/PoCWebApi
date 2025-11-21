using PoCWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace PoCWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
        public DbSet<PIITodoItem> PIITodos => Set<PIITodoItem>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<TodoItem>(e =>
            {
                e.Property(p => p.OwnerOid).HasMaxLength(64).IsRequired();
                e.Property(p => p.Title).HasMaxLength(200).IsRequired();
                e.HasIndex(p => new { p.OwnerOid, p.IsDone });
            });

            b.Entity<PIITodoItem>(e =>
            {
                e.Property(p => p.OwnerOid).HasMaxLength(64).IsRequired();
                e.Property(p => p.PIITitle).HasMaxLength(200).IsRequired();
                e.HasIndex(p => new { p.OwnerOid, p.IsDone });
            });
        }
    }
}
