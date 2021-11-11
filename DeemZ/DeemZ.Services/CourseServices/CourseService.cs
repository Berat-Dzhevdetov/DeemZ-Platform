namespace DeemZ.Services.CourseServices
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Course;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Services.LectureServices;
    using DeemZ.Global.Extensions;
    using DeemZ.Services.PromoCodeServices;
    using System.Threading.Tasks;

    public class CourseService : ICourseService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly ILectureService lectureService;
        private readonly IPromoCodeService promoCodeService;

        public CourseService(DeemZDbContext context, IMapper mapper, ILectureService lectureService, IPromoCodeService promoCodeService)
        {
            this.context = context;
            this.mapper = mapper;
            this.lectureService = lectureService;
            this.promoCodeService = promoCodeService;
        }

        public int GetUserCredits(string id)
            => context
               .Users
               .Include(x => x.Exams)
               .FirstOrDefault(x => x.Id == id)
               .Exams
               .Sum(x => x.EarnedCredits);


        public IEnumerable<T> GetUserCurrentCourses<T>(string uid, bool isPaid = true)
            => context.UserCourses
                .Where(
                    x => x.User.Id == uid
                    && x.IsPaid == isPaid
                    && x.Course.EndDate > DateTime.Now
                )
                .Include(x => x.Course)
                .Select(x => mapper.Map<T>(x.Course))
                .ToList();

        public T GetCourseById<T>(string cid)
        {
            var course = context.Courses
                .Where(x => x.Id == cid)
                .Include(x => x.Exams)
                .Include(x => x.Lectures)
                .ThenInclude(x => x.Descriptions)
                .Include(c => c.Lectures)
                .ThenInclude(x => x.Resources)
                .ThenInclude(x => x.ResourceType)
                .FirstOrDefault();

            return mapper.Map<T>(course);
        }

        public bool IsUserSignUpForThisCourse(string uid, string cid)
            => context.UserCourses
                .Any(x => x.UserId == uid && x.CourseId == cid && x.IsPaid == true);

        public IEnumerable<T> GetCoursesForSignUp<T>()
            => context.Courses
            .Where(x =>
                x.SignUpStartDate <= DateTime.UtcNow
                && x.SignUpEndDate > DateTime.UtcNow)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToList();

        

        public async Task SignUserToCourse(string uid, string cid, bool isPaid = true)
        {
            var toPay = GetCourseById<Course>(cid).Price;

            var userCourse = new UserCourse()
            {
                CourseId = cid,
                UserId = uid,
                IsPaid = isPaid,
                PaidOn = DateTime.UtcNow,
                Paid = toPay,
            };

            context.UserCourses.Add(userCourse);
            await context.SaveChangesAsync();
        }

        public async Task SignUserToCourse(string uid, string cid, SignUpCourseFormModel signUp)
        {
            var promoCode = promoCodeService.GetPromoCode(signUp.PromoCode);

            var toPay = GetCourseById<Course>(cid).Price;

            var userCourse = new UserCourse()
            {
                CourseId = cid,
                UserId = uid,
                IsPaid = true,
                PaidOn = DateTime.UtcNow,
                Paid = toPay
            };

            if(promoCode != null)
            {
                userCourse.Paid -= promoCode.DiscountPrice;
                userCourse.PromoCodeId = promoCode.Id;
                await promoCodeService.MarkPromoCodeAsUsed(promoCode.Id);
            }

            context.UserCourses.Add(userCourse);
            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetCourses<T>()
            => context.UserCourses
                .Include(x => x.User)
                .Include(x => x.Course)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public int GetUserCoursesCount() => context.UserCourses.Count();

        public async Task<string> CreateCourse(AddCourseFormModel course)
        {
            var newlyCourse = mapper.Map<Course>(course);

            context.Courses.Add(newlyCourse);
            await context.SaveChangesAsync();

            return newlyCourse.Id;
        }

        public async Task Edit(EditCourseFormModel course, string courseId)
        {
            var courseToEdit = GetCourseById<Course>(courseId);

            courseToEdit.Name = course.Name;
            courseToEdit.StartDate = course.StartDate.ToUniversalTime();
            courseToEdit.EndDate = course.EndDate.ToUniversalTime();
            courseToEdit.Credits = course.Credits;
            courseToEdit.Price = course.Price;
            courseToEdit.Description = course.Description;
            courseToEdit.SignUpStartDate = course.SignUpStartDate.ToUniversalTime();
            courseToEdit.SignUpEndDate = course.SignUpEndDate.ToUniversalTime();

            await context.SaveChangesAsync();
        }

        public bool GetCourseById(string id)
            => context.Courses.Any(x => x.Id == id);

        public async Task DeleteCourse(string cid)
        {
            var courseToDelete = GetCourseById<Course>(cid);

            context.Courses.Remove(courseToDelete);
            await context.SaveChangesAsync();
        }

        public void CreateBasicsLectures(string courseId, AddCourseFormModel course)
        {
            var basicLecturesNames = new string[] { "Resources", "Course Introduciton" };

            for (int i = 0; i < basicLecturesNames.Length; i++)
            {
                var lecture = new AddLectureFormModel
                {
                    Date = course.StartDate.AddSeconds(i),
                    Name = basicLecturesNames[i]
                };

                lectureService.AddLectureToCourse(courseId, lecture);
            }
        }

        public IEnumerable<T> GetUserCourses<T>(int page = 1, int quantity = 20)
            => context.UserCourses
                .OrderByDescending(x => x.PaidOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity);

        public IEnumerable<KeyValuePair<string, string>> GetCourseByIdAsKeyValuePair(DateTime prevDate)
            => context.Courses
                .Where(x => x.SignUpEndDate > prevDate)
                .ToDictionary(x => x.Id, x => x.Name)
                .ToList();

        public async Task DeleteUserFromCourse(string courseId, string userId)
        {
            var userCourse = context.UserCourses
                .FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId);

            if (userCourse == null) return;

            context.UserCourses.Remove(userCourse);
            await context.SaveChangesAsync();
        }

        public int UpCommingCoursesCount()
            => context.Courses
            .Where(x =>
                x.SignUpStartDate <= DateTime.UtcNow
                && x.SignUpEndDate > DateTime.UtcNow)
            .Count();
    }
}