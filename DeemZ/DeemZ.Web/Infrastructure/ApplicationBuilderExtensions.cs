namespace DeemZ.Web.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<DeemZDbContext>();

            data.Database.Migrate();

            //CreateResourceTypes(data);
            //CreateCourses(data);

            return app;
        }

        public static void CreateCourses(DeemZDbContext data)
        {
            if (data.Courses.Any()) return;
            var courses = new List<Course>()
            {
                new Course()
                {
                    Name = "ASP.NET Core - July 2021",
                    StartDate = DateTime.Parse("04.07.2021 00:00:00"),
                    EndDate = DateTime.Parse("04.07.2021 23:59:59").AddDays(67),
                    SignUpStartDate = DateTime.Parse("20.07.2021 00:00:00"),
                    SignUpEndDate = DateTime.Parse("20.07.2021 23:59:59"),
                    Lectures = new List<Lecture>()
                    {
                        new Lecture()
                        {
                            Name = "Resources",
                            Descriptions = new List<Description>(),
                            Resources = new List<Resource>()
                            {
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Youtube link"),
                                    Path = "https://www.youtube.com/watch?v=qEIy8xEhJTg",
                                    Name = "Information video for using the DeemZ training system"
                                },
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Facebook link"),
                                    Path = "https://www.facebook.com/groups/CsharpWebMay2021",
                                    Name = "Facebook group"

                                },
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Word file"),
                                    Path = "/File/View?name=00.CSharp-Web-Basics-Course-Introduction-Project-Assignment-Date-1.docx",
                                    Name = "Project Assignment - Date 1"
                                },
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Word file"),
                                    Path = "/File/View?name=00.CSharp-Web-Basics-Course-Introduction-Project-Assignment-Date-2.docx",
                                    Name = "Project Assignment - Date 2"
                                }
                            }
                        },
                        new Lecture()
                        {
                            Name = "Course Introduction",
                            Date = DateTime.Parse("04.07.2021 18:30:00"),
                            Descriptions = new List<Description>(),
                            Resources = new List<Resource>()
                            {
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Presentation"),
                                    Path = "/File/View?name=00.CSharp-ASP-NET-Core-Course-Introduction.pptx",
                                    Name = "Course Introduction - Presentation"
                                },
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Video"),
                                    Path = "/Course/Video?name=course-introduction-video-asp-dot-net-core-june-2021",
                                    Name = "Course Introduction - Video"
                                }
                            }
                        },
                        new Lecture()
                        {
                            Name = "Course Introduction",
                            Date = DateTime.Parse("04.07.2021 18:30:00"),
                            Descriptions = new List<Description>()
                            {
                                new Description()
                                {
                                    Name = "ASP.NET Core Overview",
                                },
                                new Description()
                                {
                                    Name ="Creating our first ASP.NET Core Projects"
                                }
                            },
                            Resources = new List<Resource>()
                            {
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Presentation"),
                                    Path = "/File/View?name=00.CSharp-ASP-NET-Core-Course-Introduction.pptx",
                                    Name = "Course Introduction - Presentation"
                                },
                                new Resource()
                                {
                                    ResourceType = data.ResourceTypes.FirstOrDefault(x => x.Name == "Video"),
                                    Path = "/Course/Video?name=course-introduction-video-asp-dot-net-core-june-2021",
                                    Name = "Course Introduction - Video"
                                }
                            }
                        }
                    }
                },
            };
        }
    }
}