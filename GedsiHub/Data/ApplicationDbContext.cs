using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GedsiHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your models
        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CollegeDepartment> CollegeDepartments { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Map Identity tables to custom names
            builder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("user_tbl");
                b.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                b.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                b.Property(e => e.GenderIdentity).HasMaxLength(30).IsRequired(false);
                b.Property(e => e.Pronouns).HasMaxLength(30).IsRequired(false);
                b.Property(e => e.DateOfBirth).HasColumnType("DATE").IsRequired();
                b.Property(e => e.ProfilePicturePath).HasMaxLength(200).IsRequired(false);
                b.HasIndex(e => e.Email).IsUnique();
                b.HasIndex(e => e.UserName).IsUnique();
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("role_tbl");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("user_role_tbl");
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("user_claim_tbl");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("user_login_tbl");
            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("role_claim_tbl");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("user_token_tbl");
            });

            // **Configure Certificate relationships**
            builder.Entity<Certificate>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Certificates)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(e => e.Module)
                      .WithMany(m => m.Certificates)
                      .HasForeignKey(e => e.ModuleId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure ForumComment relationships**
            builder.Entity<ForumComment>(entity =>
            {
                entity.HasOne(fc => fc.User)
                      .WithMany(u => u.ForumComments)
                      .HasForeignKey(fc => fc.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(fc => fc.ForumPost)
                      .WithMany(fp => fp.ForumComments)
                      .HasForeignKey(fc => fc.PostId)
                      .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete from ForumPost
            });

            // **Configure ForumPost relationships**
            builder.Entity<ForumPost>(entity =>
            {
                entity.HasOne(fp => fp.User)
                      .WithMany(u => u.ForumPosts)
                      .HasForeignKey(fp => fp.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure Admin relationships**
            builder.Entity<Admin>(entity =>
            {
                entity.HasOne(a => a.User)
                      .WithOne(u => u.Admin)
                      .HasForeignKey<Admin>(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure Employee relationships**
            builder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Employee)
                      .HasForeignKey<Employee>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure Student relationships**
            builder.Entity<Student>(entity =>
            {
                entity.HasOne(s => s.User)
                      .WithOne(u => u.Student)
                      .HasForeignKey<Student>(s => s.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(s => s.CollegeDepartment)
                      .WithMany(cd => cd.Students)
                      .HasForeignKey(s => s.CollegeDeptId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure Enrollment relationships**
            builder.Entity<Enrollment>(entity =>
            {
                entity.HasOne(en => en.User)
                      .WithMany(u => u.Enrollments)
                      .HasForeignKey(en => en.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(en => en.Module)
                      .WithMany(m => m.Enrollments)
                      .HasForeignKey(en => en.ModuleId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure UserProgress relationships**
            builder.Entity<UserProgress>(entity =>
            {
                entity.HasOne(up => up.User)
                      .WithMany(u => u.UserProgresses)
                      .HasForeignKey(up => up.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                entity.HasOne(up => up.Module)
                      .WithMany(m => m.UserProgresses)
                      .HasForeignKey(up => up.ModuleId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure Answer relationships**
            builder.Entity<Answer>(entity =>
            {
                entity.HasOne(a => a.Question)
                      .WithMany(q => q.Answers)
                      .HasForeignKey(a => a.QuestionId)
                      .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete from Question
            });

            // **Configure Question relationships**
            builder.Entity<Question>(entity =>
            {
                entity.HasOne(q => q.Module)
                      .WithMany(m => m.Questions)
                      .HasForeignKey(q => q.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete from Module
            });

            // **Configure Quiz relationships**
            builder.Entity<Quiz>(entity =>
            {
                entity.HasOne(qz => qz.Module)
                      .WithMany(m => m.Quizzes)
                      .HasForeignKey(qz => qz.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete from Module
            });

            // **Configure CourseContent relationships**
            builder.Entity<CourseContent>(entity =>
            {
                entity.HasOne(cc => cc.Module)
                      .WithMany(m => m.CourseContents)
                      .HasForeignKey(cc => cc.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade); // Allow cascade delete from Module
            });

            // **Configure Module relationships**
            builder.Entity<Module>(entity =>
            {
                entity.HasMany(m => m.Certificates)
                      .WithOne(c => c.Module)
                      .HasForeignKey(c => c.ModuleId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure CollegeDepartment relationships**
            builder.Entity<CollegeDepartment>(entity =>
            {
                entity.HasMany(cd => cd.Students)
                      .WithOne(s => s.CollegeDepartment)
                      .HasForeignKey(s => s.CollegeDeptId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Add configurations for other entities as necessary**
        }
    }
}
