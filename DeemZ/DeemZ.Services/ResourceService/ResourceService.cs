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
    using DeemZ.Global.Extensions;
    using System.Threading.Tasks;

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

        public async Task<string> AddResourceToLecture(string lid, string publicId, AddResourceFormModel resource)
        {
            var newlyResource = mapper.Map<Resource>(resource);
            newlyResource.LectureId = lid;
            newlyResource.PublicId = publicId;

            context.Resources.Add(newlyResource);
            await context.SaveChangesAsync();

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

        public async Task DeleteLectureResoureces(string lid)
        {
            var resources = context.Resources.Where(x => x.LectureId == lid).Include(x => x.ResourceType).ToList();

            foreach (var resource in resources)
            {
                if (!resource.ResourceType.IsRemote) fileService.DeleteFile(resource.Name);

                context.Resources.Remove(resource);
            }

            await context.SaveChangesAsync();
        }

        public async Task<string> Delete(string resourceId)
        {
            var resource = GetResourceById<Resource>(resourceId);
            var lectureId = resource.LectureId;

            var isVideo = resource.ResourceType.Name == "Video";

            if (!resource.ResourceType.IsRemote) fileService.DeleteFile(resource.PublicId, isVideo: isVideo);

            context.Resources.Remove(resource);
            await context.SaveChangesAsync();

            return lectureId;
        }

        public IEnumerable<T> GetUserResources<T>(string uid, int page, int quantity)
            => context.Resources
                .Include(x => x.Lecture)
                .ThenInclude(x => x.Course)
                .Where(x => x.Lecture.Course.UserCourses.Any(x => x.UserId == uid && x.PaidOn != null))
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public int GetUserResourcesCount(string uid)
            => context.Resources
                    .Count(x => x.Lecture.Course.UserCourses.Any(x => x.UserId == uid && x.PaidOn != null));
    }
}