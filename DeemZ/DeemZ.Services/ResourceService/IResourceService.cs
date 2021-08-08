namespace DeemZ.Services.ResourceService
{
    using DeemZ.Models.FormModels.Resource;
    using System.Collections.Generic;
    public interface IResourceService
    {
        IEnumerable<T> GetUserResources<T>(string uid);
        IEnumerable<T> GetResourceTypes<T>();
        void AddResourceToLecture(string lid, AddResourceFormModel resource);
        bool IsValidResourceType(string rtid);
        bool DoesUserHavePermissionToThisResource(string rid, string uid);
        T GetResourceById<T>(string rid);
        bool GetResourceById(string rid);
        void DeleteLectureResoureces(string lid);
        string Delete(string resourceId);
    }
}