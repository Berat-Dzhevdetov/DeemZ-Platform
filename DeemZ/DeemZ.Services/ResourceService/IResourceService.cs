namespace DeemZ.Services.ResourceService
{
    using DeemZ.Models.FormModels.Resource;
    using System.Collections.Generic;
    public interface IResourceService
    {
        IEnumerable<T> GetUserResources<T>(string uid);

        IEnumerable<T> GetResourceTypes<T>();

        void AddResourceToLecture(string lectureId, AddResourceFormModel resource);

        bool IsValidResourceType(string id);
    }
}