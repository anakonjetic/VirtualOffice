using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VirtualOffice.Models;

namespace VirtualOffice.Data
{
    public class ApplicationDbContext : IdentityDbContext
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

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1e95f075-9cbd-4252-8a25-faeb03e0449e",
                    UserName = "akonjetic@tvz.hr",
                    NormalizedUserName = "AKONJETIC@TVZ.HR",
                    Email = "akonjetic@tvz.hr",
                    NormalizedEmail = "AKONJETIC@TVZ.HR",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEHdoVZxob+hAFS7k94sF73hok2cbxnwffaz2Lh64SLJjSL4RXBfMEOhRYa4FfNRbgw==",
                    SecurityStamp = "DLH4EV4JMJXZHL7E26I56PEVVUUTTIHP",
                    ConcurrencyStamp = "4f6b5901-3484-4193-8019-eda79b7bb7c4",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "9ad46335-32b4-492d-8592-4379e0f2f108",
                    UserName = "mtkalec@tvz.hr",
                    NormalizedUserName = "MTKALEC@TVZ.HR",
                    Email = "mtkalec@tvz.hr",
                    NormalizedEmail = "MTKALEC@TVZ.HR",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAECk7W8s/aNRCGsRYsTy4OFTL6m6UtZ24akpO+00ixeCfe5jOnRa2RaUgTCgdLqE+wQ==",
                    SecurityStamp = "2P3O5IYYVMYAJPTP4SJBNZCKWICKIUFJ",
                    ConcurrencyStamp = "24c9aa1d-9f2a-46a0-86e1-a243019622b6",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "f0e2bb7d-9ef4-421d-ad33-850f079c9507",
                    UserName = "ijelinic@tvz.hr",
                    NormalizedUserName = "IJELINIC@TVZ.HR",
                    Email = "ijelinic@tvz.hr",
                    NormalizedEmail = "IJELINIC@TVZ.HR",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEDWL7FHTTTSozek+8JSsLrZVKVgj1weRDzOIbyHFxCdt2ql3B/aV21aAW36GbV3wRw==",
                    SecurityStamp = "XGHG2H7ZFL7VBGKKCEK7F62YQH7JCJP7",
                    ConcurrencyStamp = "a097299b-84ec-4ac4-a12e-f29c46f83610",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "c74eba9f-b845-4a07-b524-16333e5d0a28",
                    UserName = "lradosev1@tvz.hr",
                    NormalizedUserName = "LRADOSEV1@TVZ.HR",
                    Email = "lradosev1@tvz.hr",
                    NormalizedEmail = "LRADOSEV1@TVZ.HR",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEI8AdUEzjlWIzWCDg9b1Pxts7gJRnbdF40Z2g85lHfejyCT+iNuffZfCRphOahvbWw==",
                    SecurityStamp = "6NMWXOKUSY6BAUYR5AQXNLMCAF3GS5F6",
                    ConcurrencyStamp = "a000153f-27b3-4f1a-9239-54a0da24b5ec",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new IdentityUser
                {
                    Id = "938ea5fe-88c8-4662-9192-c3c668a7cb07",
                    UserName = "lkranjcec@tvz.hr",
                    NormalizedUserName = "LKRANJCEC@TVZ.HR",
                    Email = "lkranjcec@tvz.hr",
                    NormalizedEmail = "LKRANJCEC@TVZ.HR",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEAOqiEegzbkuyHFSWeQmTidH+MrCH86ckhw1q9ellMBVEL7LgaH+6OpeCiX+Dk7AXw==",
                    SecurityStamp = "UQGAQP2JVWKWF2FF4P44UT5WLZDAZCSX",
                    ConcurrencyStamp = "f3cd1583-e393-40cc-8bdd-c31d0b94e9ca",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }

            );


            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Ana",
                    LastName = "Konjetić",
                    DateOfBirth = new DateTime(2000, 8, 11),
                    RemainingDaysOff = 20,
                    SickLeaveDaysUsed = 5,
                    EquipmentId = "1#2#3",
                    TeamId = 1,
                    UserId = "akonjetic@tvz.hr"
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Marko",
                    LastName = "Tkalec",
                    DateOfBirth = new DateTime(1997, 2, 9),
                    RemainingDaysOff = 15,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "4#5#6",
                    TeamId = 2,
                    UserId = "mtkalec@tvz.hr"
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "Ivan",
                    LastName = "Jelinić",
                    DateOfBirth = new DateTime(2000, 6, 7),
                    RemainingDaysOff = 15,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "1#3#6",
                    TeamId = 3,
                    UserId = "ijelinic@tvz.hr"
                },
                new Employee
                {
                    Id = 4,
                    FirstName = "Marina",
                    LastName = "Marković",
                    DateOfBirth = new DateTime(1985, 9, 18),
                    RemainingDaysOff = 17,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "1#4#5",
                    TeamId = 1,
                    UserId = ""
                },
                new Employee
                {
                    Id = 5,
                    FirstName = "Ivan",
                    LastName = "Babić",
                    DateOfBirth = new DateTime(1995, 6, 25),
                    RemainingDaysOff = 19,
                    SickLeaveDaysUsed = 3,
                    EquipmentId = "2#3#6",
                    TeamId = 2,
                    UserId = ""
                },
                new Employee
                {
                    Id = 6,
                    FirstName = "Ana",
                    LastName = "Knežević",
                    DateOfBirth = new DateTime(1993, 11, 30),
                    RemainingDaysOff = 15,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "1#3#5",
                    TeamId = 3,
                    UserId = ""
                },
                new Employee
                {
                    Id = 7,
                    FirstName = "Petar",
                    LastName = "Petrović",
                    DateOfBirth = new DateTime(1987, 4, 17),
                    RemainingDaysOff = 18,
                    SickLeaveDaysUsed = 3,
                    EquipmentId = "2#4#6",
                    TeamId = 1,
                    UserId = ""
                },
                new Employee
                {
                    Id = 8,
                    FirstName = "Martina",
                    LastName = "Šimunović",
                    DateOfBirth = new DateTime(1991, 7, 9),
                    RemainingDaysOff = 16,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "3#5#6",
                    TeamId = 2,
                    UserId = ""
                },
                new Employee
                {
                    Id = 9,
                    FirstName = "Antonio",
                    LastName = "Vuković",
                    DateOfBirth = new DateTime(1989, 2, 14),
                    RemainingDaysOff = 20,
                    SickLeaveDaysUsed = 1,
                    EquipmentId = "1#4#5",
                    TeamId = 3,
                    UserId = ""
                },
                new Employee
                {
                    Id = 10,
                    FirstName = "Jelena",
                    LastName = "Matković",
                    DateOfBirth = new DateTime(1994, 10, 7),
                    RemainingDaysOff = 17,
                    SickLeaveDaysUsed = 2,
                    EquipmentId = "2#3#6",
                    TeamId = 1,
                    UserId = ""
                },
                new Employee
                {
                    Id = 11,
                    FirstName = "Ivana",
                    LastName = "Horvat",
                    DateOfBirth = new DateTime(1988, 8, 21),
                    RemainingDaysOff = 16,
                    SickLeaveDaysUsed = 3,
                    EquipmentId = "2#4#6",
                    TeamId = 2,
                    UserId = ""
                },
                new Employee
                {
                    Id = 12,
                    FirstName = "Luka",
                    LastName = "Kovač",
                    DateOfBirth = new DateTime(1992, 3, 7),
                    RemainingDaysOff = 20,
                    SickLeaveDaysUsed = 1,
                    EquipmentId = "3#5#6",
                    TeamId = 3,
                    UserId = ""
                },
                 new Employee
                 {
                     Id = 13,
                     FirstName = "Ante",
                     LastName = "Kovačić",
                     DateOfBirth = new DateTime(1990, 5, 12),
                     RemainingDaysOff = 18,
                     SickLeaveDaysUsed = 4,
                     EquipmentId = "1#2#3",
                     TeamId = 1,
                     UserId = ""
                 },
                  new Employee
                  {
                      Id = 14,
                      FirstName = "Lucija",
                      LastName = "Kranjčec",
                      DateOfBirth = new DateTime(2001, 5, 31),
                      RemainingDaysOff = 25,
                      SickLeaveDaysUsed = 1,
                      EquipmentId = "4#5#6",
                      TeamId = 5,
                      UserId = "lkranjcec@tvz.hr"
                  },
                   new Employee
                   {
                       Id = 15,
                       FirstName = "Luka",
                       LastName = "Radošević",
                       DateOfBirth = new DateTime(2000, 1, 1),
                       RemainingDaysOff = 12,
                       SickLeaveDaysUsed = 10,
                       EquipmentId = "1#2#3",
                       TeamId = 4,
                       UserId = "lradosev1@tvz.hr"
                   }

            );

            modelBuilder.Entity<EmployeeManager>().HasData(
                new EmployeeManager { ManagerId = 1, EmployeeId = 4 },
                new EmployeeManager { ManagerId = 2, EmployeeId = 5 },
                new EmployeeManager { ManagerId = 3, EmployeeId = 6 },
                new EmployeeManager { ManagerId = 1, EmployeeId = 7 },
                new EmployeeManager { ManagerId = 2, EmployeeId = 8 },
                new EmployeeManager { ManagerId = 3, EmployeeId = 9 },
                new EmployeeManager { ManagerId = 1, EmployeeId = 10 },
                new EmployeeManager { ManagerId = 2, EmployeeId = 11 },
                new EmployeeManager { ManagerId = 3, EmployeeId = 12 },
                new EmployeeManager { ManagerId = 1, EmployeeId = 13 },
                new EmployeeManager { ManagerId = 14, EmployeeId = 1 },
                new EmployeeManager { ManagerId = 14, EmployeeId = 2 },
                new EmployeeManager { ManagerId = 14, EmployeeId = 3 },
                new EmployeeManager { ManagerId = 14, EmployeeId = 15 }
             );



            base.OnModelCreating(modelBuilder);
        }
    }
}