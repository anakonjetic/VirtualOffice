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
        .HasKey(em => new { em.ManagerId, em.EmployeeId });


            modelBuilder.Entity<Request>()
                .HasOne(r => r.Employee)      
                .WithMany(e => e.Request)     
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction); ; 

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Manager)      
                .WithMany()                   
                .HasForeignKey(r => r.ManagerId)
                .OnDelete(DeleteBehavior.NoAction); ; 

            modelBuilder.Entity<EmployeeManager>()
                .HasOne(em => em.Employee)
                .WithMany(e => e.ManagedEmployees)
                .HasForeignKey(em => em.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<EmployeeManager>()
                .HasOne(em => em.Manager)
                .WithMany(m => m.ManagingEmployees)
                .HasForeignKey(em => em.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EvaluationForm>()
                .HasOne(ef => ef.Employee)
                .WithMany()
                .HasForeignKey(ef => ef.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<EvaluationForm>()
                .HasOne(ef => ef.Manager)
                .WithMany()
                .HasForeignKey(ef => ef.ManagerId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<EvaluationForm>()
                .HasOne(ef => ef.EvaluationType)
                .WithMany()
                .HasForeignKey(ef => ef.EvaluationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}