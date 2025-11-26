using Microsoft.EntityFrameworkCore;
using PoCWebApi.Auth.Models;
using PoCWebApi.Models;

namespace PoCWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
        public DbSet<PIITodoItem> PIITodos => Set<PIITodoItem>();

        // RBAC sets
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RoleService> RoleServices => Set<RoleService>();
        public DbSet<ServicePermission> ServicePermissions => Set<ServicePermission>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Existing mappings
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

            // === RBAC model ===

            b.Entity<User>(e =>
            {
                e.HasKey(x => x.Oid);
                e.Property(x => x.DisplayName).HasMaxLength(200);
                e.Property(x => x.Email).HasMaxLength(320);
            });

            b.Entity<Role>(e =>
            {
                e.HasKey(x => x.RoleId);
                e.Property(x => x.Name).HasMaxLength(128).IsRequired();
            });

            b.Entity<Service>(e =>
            {
                e.HasKey(x => x.ServiceId);
                e.Property(x => x.Key).HasMaxLength(128).IsRequired();
            });

            b.Entity<Permission>(e =>
            {
                e.HasKey(x => x.PermissionId);
                e.Property(x => x.Key).HasMaxLength(128).IsRequired();
            });

            b.Entity<UserRole>(e =>
            {
                e.HasKey(x => new { x.Oid, x.RoleId });
                e.HasOne(x => x.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(x => x.Oid);
                e.HasOne(x => x.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(x => x.RoleId);
            });

            b.Entity<RoleService>(e =>
            {
                e.HasKey(x => new { x.RoleId, x.ServiceId });
                e.HasOne(x => x.Role)
                    .WithMany(r => r.RoleServices)
                    .HasForeignKey(x => x.RoleId);
                e.HasOne(x => x.Service)
                    .WithMany(s => s.RoleServices)
                    .HasForeignKey(x => x.ServiceId);
            });

            b.Entity<ServicePermission>(e =>
            {
                e.HasKey(x => new { x.ServiceId, x.PermissionId });
                e.HasOne(x => x.Service)
                    .WithMany(s => s.ServicePermissions)
                    .HasForeignKey(x => x.ServiceId);
                e.HasOne(x => x.Permission)
                    .WithMany(p => p.ServicePermissions)
                    .HasForeignKey(x => x.PermissionId);
            });

            // === Seed some example data for PoC ===

            // Roles
            b.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "CSR", Description = "Can work with non-PII Todos only" },
                new Role { RoleId = 2, Name = "Admin", Description = "Can work with Todos and PIITodos" }
            );

            // Services
            b.Entity<Service>().HasData(
                new Service { ServiceId = 1, Key = "Todo", Description = "Standard Todo service" },
                new Service { ServiceId = 2, Key = "PIITodo", Description = "PII Todo service" }
            );

            // Permissions
            b.Entity<Permission>().HasData(
                new Permission { PermissionId = 1, Key = "Todo.Read", Description = "Read standard todos" },
                new Permission { PermissionId = 2, Key = "Todo.Write", Description = "Create/update standard todos" },
                new Permission { PermissionId = 3, Key = "PII.Read", Description = "Read PII todos" },
                new Permission { PermissionId = 4, Key = "PII.Write", Description = "Create/update PII todos" }
            );

            // ServicePermissions
            b.Entity<ServicePermission>().HasData(
                // Todo service -> Todo.Read, Todo.Write
                new ServicePermission { ServiceId = 1, PermissionId = 1 },
                new ServicePermission { ServiceId = 1, PermissionId = 2 },

                // PIITodo service -> PII.Read, PII.Write
                new ServicePermission { ServiceId = 2, PermissionId = 3 },
                new ServicePermission { ServiceId = 2, PermissionId = 4 }
            );

            // RoleServices
            b.Entity<RoleService>().HasData(
                // CSR -> Todo only
                new RoleService { RoleId = 1, ServiceId = 1 },

                // Admin -> Todo + PIITodo
                new RoleService { RoleId = 2, ServiceId = 1 },
                new RoleService { RoleId = 2, ServiceId = 2 }
            );

            // Users
            b.Entity<User>().HasData(
                new User { Oid = new Guid("a3495fa6-da05-459c-a0cf-72103747fc78"), DisplayName = "MarcoAdmin", Email = "MarcoAdmin@test.com" },
                new User { Oid = new Guid("6047da8a-10f3-4de0-b3e2-56d9ccb08573"), DisplayName = "MarcoCSR", Email = "MarcoCSR@test.com" }
            );
        }
    }
}
