namespace DeemZ.Services.ResourceService
{
    using System;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Resource;
    using DeemZ.Data.Models;
    using DeemZ.Services.FileService;

    public class ResourceService : IResourceService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public ResourceService(DeemZDbContext context, IMapper mapper, IFileService fileService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public bool IsValidResourceType(string rtid)
            => context.ResourceTypes.Any(x => x.Id == rtid);

        public string AddResourceToLecture(string lid, string publicId, AddResourceFormModel resource)
        {
            var newlyResource = mapper.Map<Resource>(resource);
            newlyResource.LectureId = lid;
            newlyResource.PublicId = publicId;

            context.Resources.Add(newlyResource);
            context.SaveChanges();

            return newlyResource.Id;
        }

        public IEnumerable<T> GetResourceTypes<T>()
            => context.ResourceTypes
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetUserResources<T>(string uid, bool isPaid = true)
            => context.Resources
                .Include(x => x.ResourceType)
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .OrderByDescending(x => x.CreatedOn)
                .Where(x =>
                    x.Lecture.Course.UserCourses.Any(c =>
                        c.UserId == uid
                        && c.CourseId == x.Lecture.CourseId
                        && c.Course.EndDate >= DateTime.Now
                        && c.IsPaid == isPaid))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public bool DoesUserHavePermissionToThisResource(string rid, string uid)
            => context.Resources
                .Where(x => x.Id == rid)
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .Any(x => x.Lecture.Course.UserCourses.Any(x => x.UserId == uid && x.IsPaid == true));

        public T GetResourceById<T>(string rid)
        {
            var resource = context.Resources
                .Include(x => x.ResourceType)
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Course)
                .FirstOrDefault(x => x.Id == rid);

            return mapper.Map<T>(resource);
        }

        public bool GetResourceById(string rid)
            => context.Resources.Any(x => x.Id == rid);

        public void DeleteLectureResoureces(string lid)
        {
            var resources = context.Resources.Where(x => x.LectureId == lid).Include(x => x.ResourceType).ToList();

            foreach (var resource in resources)
            {
                if (!resource.ResourceType.IsRemote) fileService.DeleteFile(resource.Name);

                context.Resources.Remove(resource);
            }

            context.SaveChanges();
        }

        public string Delete(string resourceId)
        {
            var resource = GetResourceById<Resource>(resourceId);
            var lectureId = resource.LectureId;

            var isVideo = resource.ResourceType.Name == "Video";

            if (!resource.ResourceType.IsRemote) fileService.DeleteFile(resource.PublicId, isVideo: isVideo);

            context.Resources.Remove(resource);
            context.SaveChanges();

            return lectureId;
        }
    }
}