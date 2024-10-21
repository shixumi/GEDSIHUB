using GedsiHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GedsiHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for the user model
        public DbSet<Student> Students { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<CollegeDepartment> CollegeDepartments { get; set; }
        public DbSet<Course> Courses { get; set; }

        // DbSets for the LMS
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; } 
        public DbSet<LessonContent> LessonContents { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        
        // DbSets for the Forum
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
        public DbSet<ForumPostReport> ForumPostReports { get; set; } 
        public DbSet<ForumCommentReport> ForumCommentReports { get; set; }

        public DbSet<FAQ> FAQs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed College Departments
            builder.Entity<CollegeDepartment>().HasData(
                new CollegeDepartment { CollegeDeptId = 1, DepartmentName = "College of Accountancy and Finance (CAF)" },
                new CollegeDepartment { CollegeDeptId = 2, DepartmentName = "College of Architecture, Design and the Built Environment (CADBE)" },
                new CollegeDepartment { CollegeDeptId = 3, DepartmentName = "College of Arts and Letters (CAL)" },
                new CollegeDepartment { CollegeDeptId = 4, DepartmentName = "College of Business Administration (CBA)" },
                new CollegeDepartment { CollegeDeptId = 5, DepartmentName = "College of Communication (COC)" },
                new CollegeDepartment { CollegeDeptId = 6, DepartmentName = "College of Computer and Information Sciences (CCIS)" },
                new CollegeDepartment { CollegeDeptId = 7, DepartmentName = "College of Education (COED)" },
                new CollegeDepartment { CollegeDeptId = 8, DepartmentName = "College of Engineering (CE)" },
                new CollegeDepartment { CollegeDeptId = 9, DepartmentName = "College of Human Kinetics (CHK)" },
                new CollegeDepartment { CollegeDeptId = 10, DepartmentName = "College of Law (CL)" },
                new CollegeDepartment { CollegeDeptId = 11, DepartmentName = "College of Political Science and Public Administration (CPSPA)" },
                new CollegeDepartment { CollegeDeptId = 12, DepartmentName = "College of Social Sciences and Development (CSSD)" },
                new CollegeDepartment { CollegeDeptId = 13, DepartmentName = "College of Science (CS)" },
                new CollegeDepartment { CollegeDeptId = 14, DepartmentName = "College of Tourism, Hospitality and Transportation Management (CTHTM)" }
            );

            // Seed Courses
            builder.Entity<Course>().HasData(
                // College of Accountancy and Finance
                new Course { CourseId = 1, CollegeDeptId = 1, CourseName = "Bachelor of Science in Accountancy (BSA)" },
                new Course { CourseId = 2, CollegeDeptId = 1, CourseName = "Bachelor of Science in Management Accounting (BSMA)" },
                new Course { CourseId = 3, CollegeDeptId = 1, CourseName = "Bachelor of Science in Business Administration Major in Financial Management (BSBAFM)" },

                // College of Architecture, Design and the Built Environment 
                new Course { CourseId = 4, CollegeDeptId = 2, CourseName = "Bachelor of Science in Architecture (BS-ARCH)" },
                new Course { CourseId = 5, CollegeDeptId = 2, CourseName = "Bachelor of Science in Interior Design (BSID)" },
                new Course { CourseId = 6, CollegeDeptId = 2, CourseName = "Bachelor of Science in Environmental Planning (BSEP)" },

                // College of Arts and Letters 
                new Course { CourseId = 7, CollegeDeptId = 3, CourseName = "Bachelor of Arts in English Language Studies (ABELS)" },
                new Course { CourseId = 8, CollegeDeptId = 3, CourseName = "Bachelor of Arts in Filipinology (ABF)" },
                new Course { CourseId = 9, CollegeDeptId = 3, CourseName = "Bachelor of Arts in Literary and Cultural Studies (ABLCS)" },
                new Course { CourseId = 10, CollegeDeptId = 3, CourseName = "Bachelor of Arts in Philosophy (AB-PHILO)" },
                new Course { CourseId = 11, CollegeDeptId = 3, CourseName = "Bachelor of Performing Arts major in Theater Arts (BPEA)" },

                // College of Business Administration
                new Course { CourseId = 12, CollegeDeptId = 4, CourseName = "Doctor in Business Administration (DBA)" },
                new Course { CourseId = 13, CollegeDeptId = 4, CourseName = "Master in Business Administration (MBA)" },
                new Course { CourseId = 14, CollegeDeptId = 4, CourseName = "Bachelor of Science in Business Administration major in Human Resource Management (BSBAHRM)" },
                new Course { CourseId = 15, CollegeDeptId = 4, CourseName = "Bachelor of Science in Business Administration major in Marketing Management (BSBA-MM)" },
                new Course { CourseId = 16, CollegeDeptId = 4, CourseName = "Bachelor of Science in Entrepreneurship (BSENTREP)" },
                new Course { CourseId = 17, CollegeDeptId = 4, CourseName = "Bachelor of Science in Office Administration (BSOA)" },

                // College of Communication 
                new Course { CourseId = 18, CollegeDeptId = 5, CourseName = "Bachelor in Advertising and Public Relations (BADPR)" },
                new Course { CourseId = 19, CollegeDeptId = 5, CourseName = "Bachelor of Arts in Broadcasting (BA Broadcasting)" },
                new Course { CourseId = 20, CollegeDeptId = 5, CourseName = "Bachelor of Arts in Communication Research (BACR)" },
                new Course { CourseId = 21, CollegeDeptId = 5, CourseName = "Bachelor of Arts in Journalism (BAJ)" },

                // College of Computer and Information Sciences 
                new Course { CourseId = 22, CollegeDeptId = 6, CourseName = "Bachelor of Science in Computer Science (BSCS)" },
                new Course { CourseId = 23, CollegeDeptId = 6, CourseName = "Bachelor of Science in Information Technology (BSIT)" },

                // College of Education 
                new Course { CourseId = 24, CollegeDeptId = 7, CourseName = "Doctor of Philsophy in Education Management (PhDEM)" },
                new Course { CourseId = 25, CollegeDeptId = 7, CourseName = "Master of Arts in Education Management (MAEM) with Specialization in: Educational Leadership, Instructional Leadership" },
                new Course { CourseId = 26, CollegeDeptId = 7, CourseName = "Master in Business Education (MBE)" },
                new Course { CourseId = 27, CollegeDeptId = 7, CourseName = "Master in Library and Information Science (MLIS)" },
                new Course { CourseId = 28, CollegeDeptId = 7, CourseName = "Master of Arts in English Language Teaching (MAELT)" },
                new Course { CourseId = 29, CollegeDeptId = 7, CourseName = "Master of Arts in Education major in Mathematics Education (MAEd-ME)" },
                new Course { CourseId = 30, CollegeDeptId = 7, CourseName = "Master of Arts in Physical Education and Sports (MAPES)" },
                new Course { CourseId = 31, CollegeDeptId = 7, CourseName = "Master of Arts in Education major in Teaching in the Challenged Areas (MAED-TCA)" },
                new Course { CourseId = 32, CollegeDeptId = 7, CourseName = "Post-Baccalaureate Diploma in Education (PBDE)" },
                new Course { CourseId = 33, CollegeDeptId = 7, CourseName = "Bachelor of Technology and Livelihood Education (BTLEd) major in: Home Economics, Industrial Arts, Information and Communication Technology"},
                new Course { CourseId = 34, CollegeDeptId = 7, CourseName = "Bachelor of Library and Information Science (BLIS)" },
                new Course { CourseId = 35, CollegeDeptId = 7, CourseName = "Bachelor of Secondary Education (BSEd) major in: English, Mathematics, Science, Filipino, Social Studies" },
                new Course { CourseId = 36, CollegeDeptId = 7, CourseName = "Bachelor of Elementary Education (BEEd)" },
                new Course { CourseId = 37, CollegeDeptId = 7, CourseName = "Bachelor of Early Childhood Education (BECEd)" },

                // College of Engineering
                new Course { CourseId = 38, CollegeDeptId = 8, CourseName = "Bachelor of Science in Civil Engineering (BSCE)" },
                new Course { CourseId = 39, CollegeDeptId = 8, CourseName = "Bachelor of Science in Computer Engineering (BSCpE)" },
                new Course { CourseId = 40, CollegeDeptId = 8, CourseName = "Bachelor of Science in Electrical Engineering (BSEE)" },
                new Course { CourseId = 41, CollegeDeptId = 8, CourseName = "Bachelor of Science in Electronics Engineering (BSECE)" },
                new Course { CourseId = 42, CollegeDeptId = 8, CourseName = "Bachelor of Science in Industrial Engineering (BSIE)" },
                new Course { CourseId = 43, CollegeDeptId = 8, CourseName = "Bachelor of Science in Mechanical Engineering (BSME)" },
                new Course { CourseId = 44, CollegeDeptId = 8, CourseName = "Bachelor of Science in Railway Engineering (BSRE)" },

                // College of Human Kinetics 
                new Course { CourseId = 45, CollegeDeptId = 9, CourseName = "Bachelor of Physical Education (BPE)" },
                new Course { CourseId = 46, CollegeDeptId = 9, CourseName = "Bachelor of Science in Exercises and Sports (BSESS)" },

                // College of Law 
                new Course { CourseId = 47, CollegeDeptId = 10, CourseName = "Juris Doctor (JD)" },

                // College of Political Science and Public Administration
                new Course { CourseId = 48, CollegeDeptId = 11, CourseName = "Doctor in Public Administration (DPA)" },
                new Course { CourseId = 49, CollegeDeptId = 11, CourseName = "Master in Public Administration (MPA)" },
                new Course { CourseId = 50, CollegeDeptId = 11, CourseName = "Bachelor of Public Administration (BPA)" },
                new Course { CourseId = 51, CollegeDeptId = 11, CourseName = "Bachelor of Arts in International Studies (BAIS)" },
                new Course { CourseId = 52, CollegeDeptId = 11, CourseName = "Bachelor of Arts in Political Economy (BAPE)" },
                new Course { CourseId = 53, CollegeDeptId = 11, CourseName = "Bachelor of Arts in Political Science (BAPS)" },

                // College of Social Sciences and Development
                new Course { CourseId = 54, CollegeDeptId = 12, CourseName = "Bachelor of Arts in History (BAH)" },
                new Course { CourseId = 55, CollegeDeptId = 12, CourseName = "Bachelor of Arts in Sociology (BAS)" },
                new Course { CourseId = 56, CollegeDeptId = 12, CourseName = "Bachelor of Science in Cooperatives (BSC)" },
                new Course { CourseId = 57, CollegeDeptId = 12, CourseName = "Bachelor of Science in Economics (BSE)" },
                new Course { CourseId = 58, CollegeDeptId = 12, CourseName = "Bachelor of Science in Psychology (BSPSY)" },

                // College of Science 
                new Course { CourseId = 59, CollegeDeptId = 13, CourseName = "Bachelor of Science Food Technology (BSFT)" },
                new Course { CourseId = 60, CollegeDeptId = 13, CourseName = "Bachelor of Science in Applied Mathematics (BSAPMATH)" },
                new Course { CourseId = 61, CollegeDeptId = 13, CourseName = "Bachelor of Science in Biology (BSBIO)" },
                new Course { CourseId = 62, CollegeDeptId = 13, CourseName = "Bachelor of Science in Chemistry (BSCHEM)" },
                new Course { CourseId = 63, CollegeDeptId = 13, CourseName = "Bachelor of Science in Mathematics (BSMATH)" },
                new Course { CourseId = 64, CollegeDeptId = 13, CourseName = "Bachelor of Science in Nutrition and Dietetics (BSND)" },
                new Course { CourseId = 65, CollegeDeptId = 13, CourseName = "Bachelor of Science in Physics (BSPHY)" },
                new Course { CourseId = 66, CollegeDeptId = 13, CourseName = "Bachelor of Science in Statistics (BSSTAT)" },

                // College of Tourism, Hospitality and Transportation Management
                new Course { CourseId = 67, CollegeDeptId = 14, CourseName = "Bachelor of Science in Hospitality Management (BSHM)" },
                new Course { CourseId = 68, CollegeDeptId = 14, CourseName = "Bachelor of Science in Tourism Management (BSTM)" },
                new Course { CourseId = 69, CollegeDeptId = 14, CourseName = "Bachelor of Science in Transportation Management (BSTRM)" }
            );

            // Seed test FAQs
            builder.Entity<FAQ>().HasData(
                new FAQ { Id = 1, Category = "General", Question = "What is GEDSI?", Answer = "GEDSI stands for Gender Equality, Diversity, and Social Inclusion. It focuses on promoting equality and inclusivity in various sectors." },
                new FAQ { Id = 2, Category = "Courses", Question = "How do I enroll in a course?", Answer = "To enroll in a course, simply navigate to the course catalog and click 'Enroll' on the course you're interested in." },
                new FAQ { Id = 3, Category = "Account", Question = "How do I reset my password?", Answer = "You can reset your password by clicking on 'Forgot Password' on the login page and following the instructions." },
                new FAQ { Id = 4, Category = "Technical Support", Question = "Why can't I log in?", Answer = "If you're having trouble logging in, please check your credentials or reset your password. If the issue persists, contact technical support." },
                new FAQ { Id = 5, Category = "Certificates", Question = "How do I get a certificate after completing a course?", Answer = "Once you've completed all the required modules and assessments, your certificate will be automatically generated and available for download in your profile." },
                new FAQ { Id = 6, Category = "Analytics", Question = "How can I view my course progress?", Answer = "You can view your course progress by going to the 'My Courses' section, where detailed analytics on your module completions will be displayed." },
                new FAQ { Id = 7, Category = "Security", Question = "How is my data protected?", Answer = "Your data is protected by our use of ASP.NET Core Data Protection, ensuring encryption at rest and in transit." },
                new FAQ { Id = 8, Category = "Modules", Question = "Can I access the final module directly?", Answer = "No, you need to complete all previous modules before accessing the final condensed learning module." },
                new FAQ { Id = 9, Category = "Forum", Question = "How can I participate in forum discussions?", Answer = "You can participate in forum discussions by navigating to the relevant course module and selecting the 'Forum' tab. Choose a topic and contribute to the discussion." },
                new FAQ { Id = 10, Category = "Courses", Question = "Can I retake a quiz if I fail?", Answer = "Yes, you can retake a quiz up to three times. After that, please contact your course administrator for further assistance." }
            );

            // Seed test Modules
            builder.Entity<Module>().HasData(
                new Module
                {
                    ModuleId = 1,
                    Title = "Introduction to Gender Equality",
                    Description = "This module covers the basics of gender equality, exploring the significance of gender equality in society and the workplace.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 2,
                    Title = "Understanding Gender Identities",
                    Description = "In this module, you'll learn about different gender identities, gender expression, and the importance of respecting personal pronouns.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 3,
                    Title = "Diversity and Inclusion in the Workplace",
                    Description = "This module discusses how diversity and inclusion can benefit organizations and create a healthier work environment.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 4,
                    Title = "Gender and Development: Global Perspectives",
                    Description = "Learn about how gender plays a role in global development, examining gender policies and frameworks used worldwide.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 5,
                    Title = "Social Inclusion Strategies",
                    Description = "This module introduces practical strategies for fostering social inclusion in various settings, from schools to workplaces.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 6,
                    Title = "Final Condensed Learning Module",
                    Description = "This is the final module summarizing all previous modules, offering an interactive format to test your knowledge and understanding.",
                    CreatedDate = DateTime.UtcNow
                },
                new Module
                {
                    ModuleId = 7,
                    Title = "Gender-Based Violence and Prevention",
                    Description = "This module educates about gender-based violence, its impact on individuals, and measures for prevention and support.",
                    CreatedDate = DateTime.UtcNow
                }
            );

            // Seed default contact information with social media and website links
            builder.Entity<ContactInfo>().HasData(
                new ContactInfo
                {
                    Id = 1,
                    SupportEmail = "dev.gedsihub@gmail.com",
                    PhoneNumber = "+1-800-123-4567",
                    Facebook = "https://www.facebook.com/gadpup",
                    TikTok = "https://www.tiktok.com/@yourprofile",
                    Instagram = "https://www.instagram.com/pupgadofficial",
                    X = "https://x.com/PUPGADO",
                    Website = "https://www.pup.edu.ph/research/gado/"
                }
            );

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

            // **Configure ForumPost created_at column with default GETDATE()**
            builder.Entity<ForumPost>()
                .Property(fp => fp.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            // **Configure ForumPost relationships**
            builder.Entity<ForumPost>(entity =>
            {
                entity.HasOne(fp => fp.User)
                      .WithMany(u => u.ForumPosts)
                      .HasForeignKey(fp => fp.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // **Configure ForumPostReport relationships**
            builder.Entity<ForumPostReport>(entity =>
            {
                entity.HasOne(pr => pr.ForumPost)
                      .WithMany(fp => fp.PostReports)
                      .HasForeignKey(pr => pr.PostId)
                      .OnDelete(DeleteBehavior.Cascade);  // Allow cascade delete from ForumPost

                entity.HasOne(pr => pr.User)
                      .WithMany(u => u.ForumPostReports) // Ensure this matches your relations
                      .HasForeignKey(pr => pr.UserId)
                      .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete from User
            });

            // **Configure ForumCommentReport relationships**
            builder.Entity<ForumCommentReport>(entity =>
            {
                entity.HasOne(cr => cr.ForumComment)
                      .WithMany(fc => fc.CommentReports)
                      .HasForeignKey(cr => cr.CommentId)
                      .OnDelete(DeleteBehavior.Cascade);  // Allow cascade delete from ForumComment

                entity.HasOne(cr => cr.User)
                      .WithMany(u => u.ForumCommentReports)  // Ensure this matches your relations
                      .HasForeignKey(cr => cr.UserId)
                      .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete from User
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

            // Module - Lesson relationship
            builder.Entity<Module>()
                .HasMany(m => m.Lessons)
                .WithOne(l => l.Module)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete Lesson when Module is deleted

            // Lesson - LessonContent relationship
            builder.Entity<Lesson>()
                .HasMany(l => l.LessonContents)
                .WithOne(lc => lc.Lesson)
                .HasForeignKey(lc => lc.LessonId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete LessonContent when Lesson is deleted

            // Configure one-to-one relationship between Module and Assessment
            builder.Entity<Module>()
                .HasOne(m => m.Assessment)
                .WithOne(a => a.Module)
                .HasForeignKey<Assessment>(a => a.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            ;

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

            
        }
    }
}
