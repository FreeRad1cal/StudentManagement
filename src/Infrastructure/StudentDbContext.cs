﻿using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class StudentDbContext: DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options): base(options)
    {
    }
    
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
    public DbSet<CourseGrade> CourseGrade { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<SchoolEnrollment> SchoolEnrollments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<SchoolYear> SchoolYears { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Course>(entity =>
        {
            entity.ToTable("Courses");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id)
                .ValueGeneratedOnAdd();
            entity.Property(c => c.Year)
                .IsRequired();
            entity.HasOne(c => c.SchoolYear)
                .WithMany()
                .HasForeignKey(c => c.Year)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(c => new { c.SchoolId, c.Year })
                .IsUnique();
        });

        builder.Entity<CourseEnrollment>(entity =>
        {
            entity.ToTable("CourseEnrollments");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedOnAdd();
            entity.HasKey(c => new { c.CourseId, c.StudentId });
            entity.HasOne(c => c.Course)
                .WithMany(c => c.CourseEnrollments)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Student)
                .WithMany(s => s.CourseEnrollments)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<CourseGrade>(entity =>
        {
            entity.ToTable("CourseGrades");
            entity.HasKey(c => new { c.CourseId, c.StudentId });
            entity.HasOne(c => c.Course)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c => c.Student)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(c => c.LetterGrade)
                .IsRequired();
            entity.HasOne(c => c.Grade)
                .WithMany()
                .HasForeignKey(c => c.LetterGrade)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<District>(entity =>
        {
            entity.ToTable("Districts");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id)
                .ValueGeneratedOnAdd();
            entity.Property(d => d.Name)
                .IsRequired();
            entity.HasIndex(d => d.Name)
                .IsUnique();
        });
        
        builder.Entity<Grade>(entity =>
        {
            entity.ToTable("Grades");
            entity.HasKey(g => g.LetterValue);
        });

        builder.Entity<School>(entity =>
        {
            entity.ToTable("Schools");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedOnAdd();
            entity.Property(s => s.Name)
                .IsRequired();
            entity.HasOne(s => s.District)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SchoolEnrollment>(entity =>
        {
            entity.ToTable("SchoolEnrollments");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedOnAdd();
            entity.HasKey(s => new { s.StudentId, s.SchoolId });
            entity.HasOne(c => c.School)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(c => c.Student)
                .WithOne(s => s.SchoolEnrollment)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<SchoolYear>(entity =>
        {
            entity.ToTable("SchoolYears");
            entity.HasKey(s => s.Year);
        });

        builder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .ValueGeneratedOnAdd();
            entity.Property(s => s.FirstName)
                .IsRequired();
            entity.Property(s => s.LastName)
                .IsRequired();
            entity.Property(s => s.DateOfBirth)
                .IsRequired();
            entity.Property(s => s.Gender)
                .IsRequired();
            entity.HasIndex(s => new { s.LastName, s.FirstName, s.DateOfBirth })
                .IsUnique();
        });
    }
}