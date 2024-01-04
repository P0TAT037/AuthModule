﻿// <auto-generated />
using System;
using AuthModule.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AuthModule.Migrations
{
    [DbContext(typeof(AuthDbContxt<User, int>))]
    [Migration("20240104141245_FirstTry")]
    partial class FirstTry
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Auth")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AuthModule.Data.Models.Claim<AuthModule.Data.Models.IUser<int>>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Claim<IUser<int>>", "Auth");
                });

            modelBuilder.Entity("AuthModule.Data.Models.Claim<AuthModule.Data.Models.User>", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Claims", "Auth");
                });

            modelBuilder.Entity("AuthModule.Data.Models.Role<AuthModule.Data.Models.IUser<int>>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role<IUser<int>>", "Auth");
                });

            modelBuilder.Entity("AuthModule.Data.Models.Role<AuthModule.Data.Models.User>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles", "Auth");
                });

            modelBuilder.Entity("AuthModule.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimId")
                        .HasColumnType("text")
                        .HasColumnName("Claim<User>Id");

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("Role<User>Id");

                    b.HasKey("Id");

                    b.HasIndex("ClaimId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users", "Auth");
                });

            modelBuilder.Entity("Claim<IUser<int>>Role<IUser<int>>", b =>
                {
                    b.Property<string>("ClaimsId")
                        .HasColumnType("text");

                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.HasKey("ClaimsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("Claim<IUser<int>>Role<IUser<int>>", "Auth");
                });

            modelBuilder.Entity("Claim<IUser<int>>User", b =>
                {
                    b.Property<string>("ClaimsId")
                        .HasColumnType("text");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("ClaimsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserClaims", "Auth");
                });

            modelBuilder.Entity("Claim<User>Role<User>", b =>
                {
                    b.Property<string>("ClaimsId")
                        .HasColumnType("text");

                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.HasKey("ClaimsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("RoleClaims", "Auth");
                });

            modelBuilder.Entity("Role<IUser<int>>User", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserRoles", "Auth");
                });

            modelBuilder.Entity("AuthModule.Data.Models.User", b =>
                {
                    b.HasOne("AuthModule.Data.Models.Claim<AuthModule.Data.Models.User>", null)
                        .WithMany("Users")
                        .HasForeignKey("ClaimId");

                    b.HasOne("AuthModule.Data.Models.Role<AuthModule.Data.Models.User>", null)
                        .WithMany("Users")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Claim<IUser<int>>Role<IUser<int>>", b =>
                {
                    b.HasOne("AuthModule.Data.Models.Claim<AuthModule.Data.Models.IUser<int>>", null)
                        .WithMany()
                        .HasForeignKey("ClaimsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthModule.Data.Models.Role<AuthModule.Data.Models.IUser<int>>", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Claim<IUser<int>>User", b =>
                {
                    b.HasOne("AuthModule.Data.Models.Claim<AuthModule.Data.Models.IUser<int>>", null)
                        .WithMany()
                        .HasForeignKey("ClaimsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthModule.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Claim<User>Role<User>", b =>
                {
                    b.HasOne("AuthModule.Data.Models.Claim<AuthModule.Data.Models.User>", null)
                        .WithMany()
                        .HasForeignKey("ClaimsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthModule.Data.Models.Role<AuthModule.Data.Models.User>", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Role<IUser<int>>User", b =>
                {
                    b.HasOne("AuthModule.Data.Models.Role<AuthModule.Data.Models.IUser<int>>", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AuthModule.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AuthModule.Data.Models.Claim<AuthModule.Data.Models.User>", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AuthModule.Data.Models.Role<AuthModule.Data.Models.User>", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}