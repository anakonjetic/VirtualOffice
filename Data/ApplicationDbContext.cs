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

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "user1", Password = "password1", Email = "user1@example.com" },
                new User { Id = 2, Username = "user2", Password = "password2", Email = "user2@example.com" }
            );

            modelBuilder.Entity<Team>().HasData(
               new Team { Id = 1, Name = "Development" },
               new Team { Id = 2, Name = "Marketing" },
               new Team { Id = 3, Name = "Finance" },
               new Team { Id = 4, Name = "Retail" },
               new Team { Id = 5, Name = "Administration" }
            );

            modelBuilder.Entity<EquipmentCategory>().HasData(
               new EquipmentCategory { Id = 1, Name = "Computers" },
               new EquipmentCategory { Id = 2, Name = "Printers and scanners" },
               new EquipmentCategory { Id = 3, Name = "Telecommunication" },
               new EquipmentCategory { Id = 4, Name = "Presentation" },
               new EquipmentCategory { Id = 5, Name = "Computer peripherals" }
           );

            modelBuilder.Entity<Equipment>().HasData(
                new Equipment
                {
                    Id = 1,
                    Name = "Laptop HP",
                    CategoryId = 1
                },
                new Equipment
                {
                    Id = 2,
                    Name = "Laptop Macbook",
                    CategoryId = 1
                },
                new Equipment
                {
                    Id = 3,
                    Name = "Laptop Lenovo",
                    CategoryId = 1
                },
                new Equipment
                {
                    Id = 4,
                    Name = "Mouse Logitech",
                    CategoryId = 5
                },
                new Equipment
                {
                    Id = 5,
                    Name = "Mouse Apple",
                    CategoryId = 5
                },
                new Equipment
                {
                    Id = 6,
                    Name = "Keyboard Logitech",
                    CategoryId = 5
                }
            );


            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RemainingDaysOff = 20,
                    SickLeaveDaysUsed = 5,
                    EquipmentId = "1#2#3",
                    TeamId = 1,
                    UserId = 1
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1995, 2, 15),
                    RemainingDaysOff = 15,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "4#5#6",
                    TeamId = 2,
                    UserId = 2
                }
            );



            base.OnModelCreating(modelBuilder);
        }
    }
}