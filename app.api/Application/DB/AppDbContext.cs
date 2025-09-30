using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using app.api.Application.Models;

namespace app.api.Application.Db;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("error_log");

            entity.HasIndex(e => e.ExceptionType, "exception_type");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("create_date");
            entity.Property(e => e.ExceptionType)
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasColumnName("exception_type");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Source)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("source");
            entity.Property(e => e.StackTrace).HasColumnName("stack_trace");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Active, "active");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.FullName, "fullname");

            entity.HasIndex(e => e.Password, "password");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("full_name");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("insert_date");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasDefaultValueSql("''")
                .HasColumnName("password");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("photo_url");
            entity.Property(e => e.UpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("update_date");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .IsFixedLength()
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_roles");

            entity.HasIndex(e => e.Id, "id");

            entity.HasIndex(e => e.Role, "role");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("insert_date");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'USER'")
                .HasColumnType("enum('USER','MODERATOR','ADM')")
                .HasColumnName("role");
            entity.Property(e => e.UptadeDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("uptade_date");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_user_roles_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}