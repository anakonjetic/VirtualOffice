﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtualOffice.Data;

#nullable disable

namespace VirtualOffice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240309173754_Users")]
    partial class Users
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VirtualOffice.Models.ClockIn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ClockInTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ClockOutTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ClockIns");
                });

            modelBuilder.Entity("VirtualOffice.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("EquipmentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RemainingDaysOff")
                        .HasColumnType("int");

                    b.Property<int>("SickLeaveDaysUsed")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Employee");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(2000, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EquipmentId = "1#2#3",
                            FirstName = "Ana",
                            LastName = "Konjetić",
                            RemainingDaysOff = 20,
                            SickLeaveDaysUsed = 5,
                            TeamId = 1,
                            UserId = "1e95f075-9cbd-4252-8a25-faeb03e0449e"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1997, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EquipmentId = "4#5#6",
                            FirstName = "Marko",
                            LastName = "Tkalec",
                            RemainingDaysOff = 15,
                            SickLeaveDaysUsed = 2,
                            TeamId = 2,
                            UserId = "9ad46335-32b4-492d-8592-4379e0f2f108"
                        },
                        new
                        {
                            Id = 3,
                            DateOfBirth = new DateTime(2000, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EquipmentId = "1#3#6",
                            FirstName = "Ivan",
                            LastName = "Jelinić",
                            RemainingDaysOff = 15,
                            SickLeaveDaysUsed = 2,
                            TeamId = 3,
                            UserId = "f0e2bb7d-9ef4-421d-ad33-850f079c9507"
                        });
                });

            modelBuilder.Entity("VirtualOffice.Models.EmployeeManager", b =>
                {
                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("ManagerId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeManager");
                });

            modelBuilder.Entity("VirtualOffice.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("EquipmentCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentCategoryId");

                    b.ToTable("Equipment");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Laptop HP"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Name = "Laptop Macbook"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            Name = "Laptop Lenovo"
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 5,
                            Name = "Mouse Logitech"
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 5,
                            Name = "Mouse Apple"
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 5,
                            Name = "Keyboard Logitech"
                        });
                });

            modelBuilder.Entity("VirtualOffice.Models.EquipmentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EquipmentCategory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Computers"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Printers and scanners"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Telecommunication"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Presentation"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Computer peripherals"
                        });
                });

            modelBuilder.Entity("VirtualOffice.Models.EvaluationForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("EvaluationTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("EvaluationTypeId1")
                        .HasColumnType("int");

                    b.Property<string>("FormDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FormTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("EvaluationTypeId");

                    b.HasIndex("EvaluationTypeId1");

                    b.HasIndex("ManagerId");

                    b.ToTable("EvaluationForm");
                });

            modelBuilder.Entity("VirtualOffice.Models.EvaluationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EvaluationType");
                });

            modelBuilder.Entity("VirtualOffice.Models.Request", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<int>("RequestTypeID")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("RequestTypeID");

                    b.HasIndex("StatusId");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("VirtualOffice.Models.RequestType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RequestType");
                });

            modelBuilder.Entity("VirtualOffice.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("VirtualOffice.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Team");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Development"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Marketing"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Finance"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Retail"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Administration"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VirtualOffice.Models.ClockIn", b =>
                {
                    b.HasOne("VirtualOffice.Models.Employee", "Employee")
                        .WithMany("ClockIn")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("VirtualOffice.Models.Employee", b =>
                {
                    b.HasOne("VirtualOffice.Models.Team", "Team")
                        .WithMany("Employee")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("VirtualOffice.Models.EmployeeManager", b =>
                {
                    b.HasOne("VirtualOffice.Models.Employee", "Employee")
                        .WithMany("ManagedEmployees")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.Employee", "Manager")
                        .WithMany("ManagingEmployees")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("VirtualOffice.Models.Equipment", b =>
                {
                    b.HasOne("VirtualOffice.Models.EquipmentCategory", "EquipmentCategory")
                        .WithMany("Equipment")
                        .HasForeignKey("EquipmentCategoryId");

                    b.Navigation("EquipmentCategory");
                });

            modelBuilder.Entity("VirtualOffice.Models.EvaluationForm", b =>
                {
                    b.HasOne("VirtualOffice.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.EvaluationType", "EvaluationType")
                        .WithMany()
                        .HasForeignKey("EvaluationTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.EvaluationType", null)
                        .WithMany("EvaluationForm")
                        .HasForeignKey("EvaluationTypeId1");

                    b.HasOne("VirtualOffice.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("EvaluationType");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("VirtualOffice.Models.Request", b =>
                {
                    b.HasOne("VirtualOffice.Models.Employee", "Employee")
                        .WithMany("Request")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.RequestType", "RequestType")
                        .WithMany("Request")
                        .HasForeignKey("RequestTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VirtualOffice.Models.Status", "Status")
                        .WithMany("Request")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Manager");

                    b.Navigation("RequestType");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("VirtualOffice.Models.Employee", b =>
                {
                    b.Navigation("ClockIn");

                    b.Navigation("ManagedEmployees");

                    b.Navigation("ManagingEmployees");

                    b.Navigation("Request");
                });

            modelBuilder.Entity("VirtualOffice.Models.EquipmentCategory", b =>
                {
                    b.Navigation("Equipment");
                });

            modelBuilder.Entity("VirtualOffice.Models.EvaluationType", b =>
                {
                    b.Navigation("EvaluationForm");
                });

            modelBuilder.Entity("VirtualOffice.Models.RequestType", b =>
                {
                    b.Navigation("Request");
                });

            modelBuilder.Entity("VirtualOffice.Models.Status", b =>
                {
                    b.Navigation("Request");
                });

            modelBuilder.Entity("VirtualOffice.Models.Team", b =>
                {
                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}