using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualOffice.Models;

namespace VirtualOffice.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
            
        public DbSet<ClockIn> ClockIns { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentCategory> EquipmentCategory { get; set; }
        public DbSet<EvaluationForm> EvaluationForm { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<EmployeeManager> EmployeeManager { get; set; }
        public DbSet<EvaluationType> EvaluationType { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<RequestType> RequestType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeManager>()
        .HasKey(em => new { em.ManagerId, em.EmployeeId }); // Define composite primary key


            modelBuilder.Entity<Request>()
                .HasOne(r => r.Employee)       // Each request is made by an employee
                .WithMany(e => e.Request)     // Each employee can have many requests
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction); ; // Foreign key

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Manager)        // Each request is handled by a manager
                .WithMany()                    // Manager can handle multiple requests
                .HasForeignKey(r => r.ManagerId)
                .OnDelete(DeleteBehavior.NoAction); ; // Foreign key

            base.OnModelCreating(modelBuilder);
        }
    }
}