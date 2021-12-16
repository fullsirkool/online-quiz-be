using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Online_Quiz.Models {
    public partial class OnlineQuizContext : DbContext {


        public OnlineQuizContext(DbContextOptions<OnlineQuizContext> options)
            : base(options) {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Answer>(entity => {
                entity.HasKey(e => new { e.QuestionId, e.AnswerId })
                    .HasName("PK__Answer__34BBBF1BABF478B9");

                entity.ToTable("Answer");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.AnswerId).HasColumnName("answerID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.IsTrue).HasColumnName("isTrue");


            });

            modelBuilder.Entity<Question>(entity => {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("userID");


            });

            modelBuilder.Entity<Result>(entity => {
                entity.HasNoKey();

                entity.ToTable("Result");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.IsPass).HasColumnName("isPass");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Result__userID__403A8C7D");
            });

            modelBuilder.Entity<Role>(entity => {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("roleID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity => {
                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("userID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
