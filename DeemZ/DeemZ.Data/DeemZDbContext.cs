namespace DeemZ.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data.Models;

    public class DeemZDbContext : IdentityDbContext<ApplicationUser>
    {
        public DeemZDbContext(DbContextOptions<DeemZDbContext> options)
            : base(options)
        {
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<InformativeMessage> InformativeMessages { get; set; }
        public DbSet<ApplicationUserExam> ApplicationUserExams { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=DeemZ;Integrated Security=true;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<UserCourse>(x =>
            {
                x.HasKey(x => new { x.CourseId, x.UserId });
            });

            mb.Entity<ApplicationUserExam>(x =>
            {
                x.HasKey(x => new { x.ExamId, x.ApplicationUserId });
            });

            base.OnModelCreating(mb);
        }
    }
}
