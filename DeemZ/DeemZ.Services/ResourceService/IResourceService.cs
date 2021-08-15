﻿namespace DeemZ.Services.ResourceService
{
    using DeemZ.Models.FormModels.Resource;
    using System.Collections.Generic;
    public interface IResourceService
    {
        IEnumerable<T> GetUserResources<T>(string uid, bool isPaid = true);
        IEnumerable<T> GetResourceTypes<T>();
        string AddResourceToLecture(string lid, string publicId, AddResourceFormModel resource);
        bool IsValidResourceType(string rtid);
        bool DoesUserHavePermissionToThisResource(string rid, string uid);
        T GetResourceById<T>(string rid);
        bool GetResourceById(string rid);
        void DeleteLectureResoureces(string lid);
        string Delete(string resourceId);
    }
}