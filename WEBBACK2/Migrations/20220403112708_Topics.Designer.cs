﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WEBBACK2.Models.Data;

#nullable disable

namespace WEBBACK2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220403112708_Topics")]
    partial class Topics
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WEBBACK2.Models.RoleDir.Role", b =>
                {
                    b.Property<int>("roleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("roleId"), 1L, 1);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("roleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WEBBACK2.Models.Solution", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("authorId")
                        .HasColumnType("int");

                    b.Property<int>("programmingLanguage")
                        .HasColumnType("int");

                    b.Property<string>("sourceCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("taskId")
                        .HasColumnType("int");

                    b.Property<int>("verdict")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("authorId");

                    b.HasIndex("taskId");

                    b.ToTable("Solutions");
                });

            modelBuilder.Entity("WEBBACK2.Models.TaskDir.Task1", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("input")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isDraft")
                        .HasColumnType("bit");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("output")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int>("topicId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("topicId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("WEBBACK2.Models.TopicDir.Topic", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("parentId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("parentId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("WEBBACK2.Models.UserDir.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("userId"), 1L, 1);

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.Property<string>("surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("userId");

                    b.HasIndex("roleId");

                    b.HasIndex("userName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WEBBACK2.Models.Solution", b =>
                {
                    b.HasOne("WEBBACK2.Models.UserDir.User", null)
                        .WithMany()
                        .HasForeignKey("authorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WEBBACK2.Models.TaskDir.Task1", null)
                        .WithMany()
                        .HasForeignKey("taskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WEBBACK2.Models.TaskDir.Task1", b =>
                {
                    b.HasOne("WEBBACK2.Models.TopicDir.Topic", null)
                        .WithMany()
                        .HasForeignKey("topicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WEBBACK2.Models.TopicDir.Topic", b =>
                {
                    b.HasOne("WEBBACK2.Models.TopicDir.Topic", null)
                        .WithMany()
                        .HasForeignKey("parentId");
                });

            modelBuilder.Entity("WEBBACK2.Models.UserDir.User", b =>
                {
                    b.HasOne("WEBBACK2.Models.RoleDir.Role", null)
                        .WithMany()
                        .HasForeignKey("roleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
