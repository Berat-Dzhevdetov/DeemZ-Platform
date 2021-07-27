namespace DeemZ.Web.Infrastructure
{
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Web.DTO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using static Constants;

    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedRoles(services);
            CreateResourceTypes(services);
            CreateCourses(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<DeemZDbContext>();

            data.Database.Migrate();
        }

        private static void CreateResourceTypes(IServiceProvider services)
        {
            var data = services.GetRequiredService<DeemZDbContext>();

            if (data.ResourceTypes.Any()) return;
            data.ResourceTypes.AddRange(new[]
            {
                new ResourceType() { Name = "Youtube link", Icon = "<i class=\"fab fa-youtube\"></i>" },
                new ResourceType() { Name = "Facebook link", Icon = "<i class=\"fab fa-facebook\"></i>" },
                new ResourceType() { Name = "Word file", Icon = "<i class=\"fas fa-file-word\"></i>" },
                new ResourceType() { Name = "Presentation", Icon = "<i class=\"fas fa-file-powerpoint\"></i>" },
                new ResourceType() { Name = "Video", Icon = "<i class=\"fas fa-video\"></i>" }
            });

            data.SaveChanges();
        }

        private static void SeedRoles(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                    return;

                var role = new IdentityRole { Name = AdminRoleName };

                await roleManager.CreateAsync(role);

                var adminPass = "admin12";

                var user = new ApplicationUser()
                {
                    Email = "admin@deemz.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                };

                await userManager.CreateAsync(user, adminPass);

                await userManager.AddToRoleAsync(user, role.Name);
            })
            .GetAwaiter()
            .GetResult();
        }

        private static void CreateCourses(IServiceProvider services)
        {
            var data = services.GetRequiredService<DeemZDbContext>();

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