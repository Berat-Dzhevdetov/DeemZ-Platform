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

    public class CourseService : ICourseService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly ILectureService lectureService;

        public CourseService(DeemZDbContext context, IMapper mapper, ILectureService lectureService)
        {
            this.context = context;
            this.mapper = mapper;
            this.lectureService = lectureService;
        }

        public int GetUserCredits(string id)
            => context
               .Users
               .Include(x => x.Exams)
               .FirstOrDefault(x => x.Id == id)
               .Exams.Sum(x => x.EarnedCredits);


        //Gets user's given id courses
        public IEnumerable<T> GetUserCurrentCourses<T>(string uid, bool isPaid = true)
         => context.UserCourses
                .Where(
                    x => x.User.Id == uid
                    && x.IsPaid == isPaid
                    && x.Course.EndDate > DateTime.UtcNow
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

        public void SignUserToCourse(string uid, string cid)
        {
            var userCourse = new UserCourse()
            {
                CourseId = cid,
                UserId = uid,
                IsPaid = true,
                PaidOn = DateTime.UtcNow
            };

            context.UserCourses.Add(userCourse);
            context.SaveChanges();
        }

        public IEnumerable<T> GetCourses<T>()
            => context.UserCourses
                .Include(x => x.User)
                .Include(x => x.Course)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public int GetUserCoursesCount() => context.UserCourses.Count();

        public string CreateCourse(AddCourseFormModel course)
        {
            var newlyCourse = mapper.Map<Course>(course);

            context.Courses.Add(newlyCourse);
            context.SaveChanges();

            return newlyCourse.Id;
        }

        public void Edit(EditCourseFormModel course, string courseId)
        {
            var courseToEdit = GetCourseById<Course>(courseId);

            courseToEdit.Name = course.Name;
            courseToEdit.StartDate = course.StartDate.ToUniversalTime();
            courseToEdit.EndDate = course.EndDate.ToUniversalTime();
            courseToEdit.Credits = course.Credits;
            courseToEdit.Price = course.Price;
            courseToEdit.SignUpStartDate = course.SignUpStartDate.ToUniversalTime();
            courseToEdit.SignUpEndDate = course.SignUpEndDate.ToUniversalTime();

            context.SaveChanges();
        }

        public bool GetCourseById(string id)
            => context.Courses.Any(x => x.Id == id);

        public void DeleteCourse(string cid)
        {
            var courseToDelete = GetCourseById<Course>(cid);

            context.Courses.Remove(courseToDelete);
            context.SaveChanges();
        }

        public void CreateBasicsLectures(string courseId, AddCourseFormModel course)
        {
            var basicLecturesNames = new string[] { "Course Introduciton", "Resources" };

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
    }
}