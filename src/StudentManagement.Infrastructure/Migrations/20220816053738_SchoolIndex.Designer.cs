﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentManagement.Infrastructure;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    [DbContext(typeof(StudentDbContext))]
    [Migration("20220816053738_SchoolIndex")]
    partial class SchoolIndex
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("StudentManagement.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Year");

                    b.HasIndex("SchoolId", "Name", "Year")
                        .IsUnique();

                    b.ToTable("Courses", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.CourseEnrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("CourseId", "StudentId")
                        .IsUnique();

                    b.ToTable("CourseEnrollments", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.CourseGrade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("LetterGrade")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LetterGrade");

                    b.HasIndex("StudentId");

                    b.HasIndex("CourseId", "StudentId")
                        .IsUnique();

                    b.ToTable("CourseGrades", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Districts", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.Grade", b =>
                {
                    b.Property<string>("LetterValue")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LetterValue");

                    b.ToTable("Grades", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("Name", "DistrictId")
                        .IsUnique();

                    b.ToTable("Schools", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.SchoolEnrollment", b =>
                {
                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("StudentId", "SchoolId");

                    b.HasIndex("SchoolId");

                    b.HasIndex("StudentId")
                        .IsUnique();

                    b.ToTable("SchoolEnrollments", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.SchoolYear", b =>
                {
                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Year");

                    b.ToTable("SchoolYears", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LastName", "FirstName", "DateOfBirth", "MiddleName")
                        .IsUnique();

                    b.ToTable("Students", (string)null);
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.Course", b =>
                {
                    b.HasOne("StudentManagement.Core.Entities.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Core.Entities.SchoolYear", "SchoolYear")
                        .WithMany()
                        .HasForeignKey("Year")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("School");

                    b.Navigation("SchoolYear");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.CourseEnrollment", b =>
                {
                    b.HasOne("StudentManagement.Core.Entities.Course", "Course")
                        .WithMany("CourseEnrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Core.Entities.Student", "Student")
                        .WithMany("CourseEnrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.CourseGrade", b =>
                {
                    b.HasOne("StudentManagement.Core.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagement.Core.Entities.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("LetterGrade")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StudentManagement.Core.Entities.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Grade");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.School", b =>
                {
                    b.HasOne("StudentManagement.Core.Entities.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.SchoolEnrollment", b =>
                {
                    b.HasOne("StudentManagement.Core.Entities.School", "School")
                        .WithMany()
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StudentManagement.Core.Entities.Student", "Student")
                        .WithOne("SchoolEnrollment")
                        .HasForeignKey("StudentManagement.Core.Entities.SchoolEnrollment", "StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.Course", b =>
                {
                    b.Navigation("CourseEnrollments");
                });

            modelBuilder.Entity("StudentManagement.Core.Entities.Student", b =>
                {
                    b.Navigation("CourseEnrollments");

                    b.Navigation("SchoolEnrollment")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
