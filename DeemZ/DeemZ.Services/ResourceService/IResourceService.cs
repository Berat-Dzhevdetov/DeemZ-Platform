namespace DeemZ.Services.ResourceService
{
    using DeemZ.Models.FormModels.Resource;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IResourceService
    {
        IEnumerable<T> GetUserResources<T>(string uid, bool isPaid = true);
        IEnumerable<T> GetResourceTypes<T>();
        Task<string> AddResourceToLecture(string lid, string publicId, AddResourceFormModel resource);
        bool IsValidResourceType(string rtid);
        bool DoesUserHavePermissionToThisResource(string rid, string uid);
        T GetResourceById<T>(string rid);
        bool GetResourceById(string rid);
        Task DeleteLectureResoureces(string lid);
        Task<string> Delete(string resourceId);
        IEnumerable<T> GetUserResources<T>(string uid, int page, int quantity);
        int GetUserResourcesCount(string uid);
    }
}