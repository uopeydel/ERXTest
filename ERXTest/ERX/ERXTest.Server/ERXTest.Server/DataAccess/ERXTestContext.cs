using System;
using ERXTest.Shared.DBModels;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; 

#nullable disable

namespace ERXTest.Server.DataAccess
{
    public partial class ERXTestContext : DbContext
    {
        private readonly string _connectionString;
        public ERXTestContext()
        {
        }
        public ERXTestContext(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ERXTestContext>>();
#pragma warning disable EF1001 // Internal EF Core API usage.
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
#pragma warning restore EF1001 // Internal EF Core API usage.
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<DropDown> DropDowns { get; set; }
        public virtual DbSet<DropDownItem> DropDownItems { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Respondent> Respondents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.EndAnswer).HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.DropDown)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.DropDownId)
                    .HasConstraintName("FK_Answer_DropDown");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<DropDown>(entity =>
            {
                entity.ToTable("DropDown");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<DropDownItem>(entity =>
            {
                entity.ToTable("DropDownItem");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.DropDown)
                    .WithMany(p => p.DropDownItems)
                    .HasForeignKey(d => d.DropDownId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DropDownItem_DropDown");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Respondent>(entity =>
            {
                entity.ToTable("Respondent");

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.AnswerName)
                  .IsRequired()
                  .HasMaxLength(255);

                entity.Property(e => e.QuestionName)
                  .IsRequired()
                  .HasMaxLength(500);

                entity.HasOne(d => d.AnswerNavigation)
                    .WithMany(p => p.Respondents)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Respondent_Answer");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Respondents)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Respondent_Question");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
