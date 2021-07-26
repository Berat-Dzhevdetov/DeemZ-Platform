namespace DeemZ.Services.ResourceService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DeemZ.Data;
    using Microsoft.EntityFrameworkCore;

    public class ResourceService : IResourceService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ResourceService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<T> GetResourceTypes<T>()
            => context.ResourceTypes
                .Select(x => mapper.Map<T>(x))
                .ToList();

        public IEnumerable<T> GetUserResources<T>(string uid)
            => context.Resources
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .OrderBy(x => x.CreatedOn)
                .Where(x =>
                    x.Lecture.Course.UserCourses.Any(c =>
                        c.UserId == uid
                        && c.CourseId == x.Lecture.CourseId
                        && c.Course.StartDate <= DateTime.UtcNow
                        && c.Course.EndDate >= DateTime.UtcNow
                        && c.IsPaid == true))
                .Select(x => mapper.Map<T>(x))
                .ToList();
    }
}
