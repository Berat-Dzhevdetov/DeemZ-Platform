namespace DeemZ.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;
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
        public DbSet<PromoCode> PromoCodes { get; set; }
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
        public DbSet<InformativeMessagesHeading> InformativeMessagesHeadings { get; set; }
        public DbSet<ApplicationUserExam> ApplicationUserExams { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<ApplicationUserSurvey> ApplicationUserSurveys { get; set; }
        public DbSet<ApplicationUserSurveyAnswer> ApplicationUserSurveyAnswers { get; set; }
        public DbSet<AnswerUsers> AnswerUsers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Partner> Partners { get; set; }


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

            mb.Entity<ApplicationUserSurveyAnswer>(x =>
            {
                x.HasKey(x => new { x.ApplicationUserId, x.SurveyAnswerId });
            });

            mb.Entity<ApplicationUserSurvey>(x =>
            {
                x.HasKey(x => new { x.ApplicationUserId, x.SurveyId });
            });

            mb.Entity<AnswerUsers>(x =>
            {
                x.HasKey(x => new { x.AnswerId, x.UserId });
            });

            mb.Entity<Comment>()
                .HasOne(x => x.Forum)
                .WithMany(b => b.Comments)
                .OnDelete(DeleteBehavior.NoAction);

            mb.Entity<Course>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,4)");

            mb.Entity<UserCourse>()
                .Property(x => x.Paid)
                .HasColumnType("decimal(18,4)");

            mb.Entity<PromoCode>()
                .Property(x => x.DiscountPrice)
                .HasColumnType("decimal(18,4)");

            base.OnModelCreating(mb);
        }

        public dynamic FindEntity(string table, string Id)
        {
            PropertyInfo prop = GetType().GetProperty(table, BindingFlags.Instance | BindingFlags.Public);
            dynamic dbSet = prop.GetValue(this, null);
            return dbSet.Find(Id);
        }
    }
}
