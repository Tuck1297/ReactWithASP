﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReactWithASP.Server.Data;

#nullable disable

namespace ReactWithASP.Server.Migrations.Data
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReactWithASP.Server.Models.ConnectionStrings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("currentTableInteracting")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("dbConnectionString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("dbName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("dbType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("dbName")
                        .IsUnique();

                    b.ToTable("ConnectionStrings");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8e2a76d6-a6d2-4fcf-a742-b878a5da2b83"),
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            currentTableInteracting = "",
                            dbConnectionString = "host=127.0.0.1; database=SupplyChain; port=5420; user id=postgres; password=123456;",
                            dbName = "SupplyChain",
                            dbType = "Postgres"
                        },
                        new
                        {
                            Id = new Guid("3740e256-5da4-454b-aac9-51a45eaac97a"),
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            currentTableInteracting = "",
                            dbConnectionString = "host=127.0.0.1; database=WebsiteInfo; port=5420; user id=postgres; password=123456;",
                            dbName = "WebsiteInfo",
                            dbType = "Postgres"
                        });
                });

            modelBuilder.Entity("ReactWithASP.Server.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            Email = "test1@hackathon.com",
                            FirstName = "TestAdmin",
                            LastName = "Tester",
                            Role = "Admin"
                        },
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"),
                            Email = "test2@hackathon.com",
                            FirstName = "TestUser",
                            LastName = "Tester",
                            Role = "User"
                        });
                });

            modelBuilder.Entity("ReactWithASP.Server.Models.UserAccount", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("UserId");

                    b.ToTable("UserAccount");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            Email = "test1@hackathon.com",
                            PasswordHash = "$2a$11$q1udDZHzUH2s37H.WNEmVupM/0siYeHWWJ.zYIS40r34mDGV1kd0i",
                            RefreshToken = "J19S5PwCdOsc152kT2T0r9Hx5WEBKTo6rNbm3GVTGAwdAsDfvGQcOHzxBe+qt7ls8S1SlyvT5VtFWUKKquAfMQ==",
                            TokenCreated = new DateTime(2023, 12, 2, 2, 16, 44, 848, DateTimeKind.Utc).AddTicks(696),
                            TokenExpires = new DateTime(2023, 12, 2, 2, 46, 44, 848, DateTimeKind.Utc).AddTicks(691)
                        },
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"),
                            Email = "test2hackathon.com",
                            PasswordHash = "$2a$11$uramN.Jwempnqmbt.T/aVeHnWiShr9VtcP3qC3mY5Y2eD2bBuiPYW",
                            RefreshToken = "4LlQcMOp/jizlaolyy029YtgVsEFAnQ9iYnd0MHIlDyJca53XNwbLVGzeSe9XkFFBjcUqGs5t1TQzyDeE/7J/A==",
                            TokenCreated = new DateTime(2023, 12, 2, 2, 16, 44, 948, DateTimeKind.Utc).AddTicks(1797),
                            TokenExpires = new DateTime(2023, 12, 2, 2, 46, 44, 948, DateTimeKind.Utc).AddTicks(1789)
                        });
                });

            modelBuilder.Entity("ReactWithASP.Server.Models.ConnectionStrings", b =>
                {
                    b.HasOne("ReactWithASP.Server.Models.User", "User")
                        .WithMany("ConnectionStrings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReactWithASP.Server.Models.User", b =>
                {
                    b.Navigation("ConnectionStrings");
                });
#pragma warning restore 612, 618
        }
    }
}
