using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BH_SocialNetwork_Models.Modelss
{
    public partial class BH_SocialNetwork_DBContext : DbContext
    {
        public BH_SocialNetwork_DBContext()
        {
        }

        public BH_SocialNetwork_DBContext(DbContextOptions<BH_SocialNetwork_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Favotired> Favotired { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersFollowing> UsersFollowing { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-P1BN3VG\\SQLEXPRESS;Initial Catalog=BH_SocialNetwork_DB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articles>(entity =>
            {
                entity.Property(e => e.ArticlesId)
                    .HasColumnName("ArticlesID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Body).HasColumnType("text");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.FavotiredCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Articles_Users");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId)
                    .HasColumnName("CommentID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArticlesId).HasColumnName("ArticlesID");

                entity.Property(e => e.Body).HasColumnType("text");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Articles)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.ArticlesId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Comment_Articles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Comment_Users");
            });

            modelBuilder.Entity<Favotired>(entity =>
            {
                entity.Property(e => e.FavotiredId)
                    .HasColumnName("FavotiredID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArticlesId).HasColumnName("ArticlesID");

                entity.Property(e => e.Favotired1)
                    .HasColumnName("Favotired")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Articles)
                    .WithMany(p => p.Favotired)
                    .HasForeignKey(d => d.ArticlesId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Favotired_Articles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favotired)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_Favotired_Users");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId)
                    .HasColumnName("TagID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArticlesId).HasColumnName("ArticlesID");

                entity.Property(e => e.TagName).HasMaxLength(50);

                entity.HasOne(d => d.Articles)
                    .WithMany(p => p.Tag)
                    .HasForeignKey(d => d.ArticlesId)
                    .HasConstraintName("fk_Tag_Articles");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Img).HasColumnType("text");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<UsersFollowing>(entity =>
            {
                entity.HasKey(e => e.UserFollowId);

                entity.ToTable("Users_Following");

                entity.Property(e => e.UserFollowId)
                    .HasColumnName("User_FollowID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.State).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.UserFollowNavigation)
                    .WithMany(p => p.UsersFollowingUserFollowNavigation)
                    .HasForeignKey(d => d.UserFollow)
                    .HasConstraintName("FK_Users_Following_Users1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersFollowingUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Users_Following_Users");
            });
        }
    }
}
