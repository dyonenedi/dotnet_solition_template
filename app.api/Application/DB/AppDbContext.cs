using Microsoft.EntityFrameworkCore;
using app.api.Application.Models;

namespace app.api.Application.DB;

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

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostLike> PostLikes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("error_log")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

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

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("post")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Active, "active");

            entity.HasIndex(e => e.Id, "id");

            entity.HasIndex(e => e.InsertDate, "insert_date");

            entity.HasIndex(e => e.UpdateDate, "update_date");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("insert_date");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");
            entity.Property(e => e.UpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("update_date");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("post_ibfk_1");
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("post_like")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.PostId, "FK_post_like_post");

            entity.HasIndex(e => e.Active, "active");

            entity.HasIndex(e => new { e.LikerId, e.PostId, e.Active }, "idx_covering");

            entity.HasIndex(e => e.InsertDate, "insert_date");

            entity.HasIndex(e => new { e.LikerId, e.PostId }, "unique").IsUnique();

            entity.HasIndex(e => e.UpdateDate, "update_date");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("active");
            entity.Property(e => e.InsertDate)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("insert_date");
            entity.Property(e => e.LikerId)
                .HasColumnType("int(11)")
                .HasColumnName("liker_id");
            entity.Property(e => e.PostId)
                .HasColumnType("int(11)")
                .HasColumnName("post_id");
            entity.Property(e => e.UpdateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Liker).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.LikerId)
                .HasConstraintName("post_like_ibfk_2");

            entity.HasOne(d => d.Post).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("post_like_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("user")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Active, "active");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.FullName, "fullname");

            entity.HasIndex(e => e.InsertDate, "insert_date");

            entity.HasIndex(e => e.Password, "password");

            entity.HasIndex(e => e.UpdateDate, "update_date");

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

            entity
                .ToTable("user_role")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.Id, "id");

            entity.HasIndex(e => e.InsertDate, "insert_date");

            entity.HasIndex(e => e.Role, "role");

            entity.HasIndex(e => e.UptadeDate, "uptade_date");

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
                .HasConstraintName("user_role_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
