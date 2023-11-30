﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReactWithASP.Server.Data;

#nullable disable

namespace ReactWithASP.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231130060537_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("dbEncryptedConnectionString")
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
                            Id = new Guid("896d1ec4-e5ff-4354-b7da-2c91453ffcc5"),
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            currentTableInteracting = "",
                            dbEncryptedConnectionString = "host=127.0.0.1; database=HackathonDB; port=5420; user id=postgres; password=123456;",
                            dbName = "HackathonDB",
                            dbType = "Postgres"
                        },
                        new
                        {
                            Id = new Guid("dc6ab834-d615-4d42-979c-41c7226b5f7a"),
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538f45"),
                            currentTableInteracting = "",
                            dbEncryptedConnectionString = "host=127.0.0.1; database=exploremoreusa; port=5420; user id=postgres; password=123456;",
                            dbName = "ExploreMoreUSA",
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
                            Email = "dev@tuckerjohnson.me",
                            FirstName = "Tucker",
                            LastName = "Johnson",
                            Role = "Admin"
                        },
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"),
                            Email = "hashtimemail@gmail.com",
                            FirstName = "Tucker",
                            LastName = "Johnson",
                            Role = "Admin"
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
                            Email = "dev@tuckerjohnson.me",
                            PasswordHash = "$2a$11$LgI.5ktKRsk0QMu.44A14uvOPtpVQGlD7ZnBlh4Y69iC/qOG6Gdv.",
                            RefreshToken = "W9SZymTk2DcJ/7wNWMQSWPeqorpj5WKosTDao31WXTWGLw9jfFuJFvmsb/Tn8dGlTuhW5QAb/F7t7lmmMzhIkA==",
                            TokenCreated = new DateTime(2023, 11, 30, 6, 5, 37, 495, DateTimeKind.Utc).AddTicks(9720),
                            TokenExpires = new DateTime(2023, 11, 30, 6, 35, 37, 495, DateTimeKind.Utc).AddTicks(9710)
                        },
                        new
                        {
                            UserId = new Guid("d63f0ca3-e25d-4583-9354-57f110538a55"),
                            Email = "hashtimemail@gmail.com",
                            PasswordHash = "$2a$11$Zizwdtseq053HXE2d95.suv.eVrKy0kxa6qOmGxsGdHbVUYh.8sGS",
                            RefreshToken = "mk8ONqTPHcWfq5oXRRWEoZVavZ+Q3vgP2aLy2tIwCX5uHf229sa0or+t6N2MXzlQLCwVX0cNYVRHwWGoSDeNbA==",
                            TokenCreated = new DateTime(2023, 11, 30, 6, 5, 37, 596, DateTimeKind.Utc).AddTicks(9399),
                            TokenExpires = new DateTime(2023, 11, 30, 6, 35, 37, 596, DateTimeKind.Utc).AddTicks(9393)
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