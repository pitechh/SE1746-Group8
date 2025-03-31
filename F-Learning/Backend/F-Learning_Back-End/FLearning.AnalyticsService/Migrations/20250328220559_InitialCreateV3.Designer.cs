﻿// <auto-generated />
using System;
using FLearning.AnalyticsService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FLearning.AnalyticsService.Migrations
{
    [DbContext(typeof(AnalyticsDbContext))]
    [Migration("20250328220559_InitialCreateV3")]
    partial class InitialCreateV3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.31")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FLearning.AnalyticsService.Models.AdminRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AdminRoles");
                });

            modelBuilder.Entity("FLearning.AnalyticsService.Models.CourseStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ActiveLearners")
                        .HasColumnType("int");

                    b.Property<DateTime>("CalculationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CompletionRate")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("TotalEnrollments")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalRevenue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId", "CalculationDate")
                        .IsUnique();

                    b.ToTable("CourseStatistics");
                });

            modelBuilder.Entity("FLearning.AnalyticsService.Models.UserActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("InteractionType")
                        .HasColumnType("int");

                    b.Property<int?>("LessonId")
                        .HasColumnType("int");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Timestamp");

                    b.ToTable("UserActivities");
                });

            modelBuilder.Entity("FLearning.AnalyticsService.Models.UserProgress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CompletedLessons")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastActivity")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalLessons")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "CourseId")
                        .IsUnique();

                    b.ToTable("UserProgresses");
                });
#pragma warning restore 612, 618
        }
    }
}
