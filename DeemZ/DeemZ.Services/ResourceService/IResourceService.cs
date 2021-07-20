namespace DeemZ.Services.ResourceService
{
    using System.Collections.Generic;
    public interface IResourceService
    {
        IEnumerable<T> GetCourseRecourses<T>(string cid);
        IEnumerable<T> GetUserResources<T>(string uid);
    }
}