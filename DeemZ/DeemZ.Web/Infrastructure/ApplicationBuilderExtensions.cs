namespace DeemZ.Web.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Web.DTO;
    using System;

    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<DeemZDbContext>();

            data.Database.Migrate();

            CreateResourceTypes(data);
            CreateCourses(data);

            return app;
        }

        private static void CreateResourceTypes(DeemZDbContext data)
        {
            if (data.ResourceTypes.Any()) return;
            data.ResourceTypes.AddRange(new[]
            {
                new ResourceType() { Name= "Youtube link" },
                new ResourceType() { Name= "Facebook link" },
                new ResourceType() { Name= "Word file" },
                new ResourceType() { Name= "Presentation" },
                new ResourceType() { Name= "Video" },
                new ResourceType() { Name= "Exam" }
            });

            data.SaveChanges();
        }

        public static void CreateCourses(DeemZDbContext data)
        {
            if (data.Courses.Any()) return;

            var json = File.ReadAllText("./importCourseData.json");

            var courses = JsonConvert.DeserializeObject<List<CourseImportDataDTO>>(json);

            var dataCourses = new List<Course>();

            foreach (var course in courses)
            {
                var newlyCourse = new Course()
                {
                    Name = course.Name,
                    SignUpEndDate = DateTime.Parse(course.SignUpEndDate),
                    EndDate = DateTime.Parse(course.EndDate),
                    StartDate = DateTime.Parse(course.StartDate),
                    SignUpStartDate = DateTime.Parse(course.SignUpStartDate),
                    Credits = course.Credits
                };

                var lectures = new List<Lecture>();

                foreach (var lecture in course.Lectures)
                {
                    var newlyLecture = new Lecture()
                    {
                        Name = lecture.Name,
                        Date = lecture.Date == null ? null : DateTime.Parse(lecture.Date),
                    };

                    foreach (var description in lecture.Descriptions)
                    {
                        newlyLecture.Descriptions.Add(new Description()
                        {
                            Name = description
                        });
                    }

                    foreach (var resource in lecture.Resources)
                    {
                        newlyLecture.Resources.Add(new Resource()
                        {
                            Name = resource.Name,
                            Path = resource.Path,
                            ResourceTypeId = data.ResourceTypes.FirstOrDefault(x => x.Name == resource.ResourceType).Id
                        });
                    }
                    lectures.Add(newlyLecture);
                }

                newlyCourse.Lectures = lectures;

                dataCourses.Add(newlyCourse);
            }

            data.Courses.AddRange(dataCourses);
            data.SaveChanges();
        }
    }
}