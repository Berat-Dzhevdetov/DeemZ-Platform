namespace DeemZ.Services.ResourceService
{
    using System;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Resource;
    using DeemZ.Data.Models;

    public class ResourceService : IResourceService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ResourceService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool IsValidResourceType(string id)
            => context.ResourceTypes.Any(x => x.Id == id);

        public void AddResourceToLecture(string lectureId, AddResourceFormModel resource)
        {
            var newlyResource = mapper.Map<Resource>(resource);
            newlyResource.LectureId = lectureId;

            context.Resources.Add(newlyResource);
            context.SaveChanges();
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
