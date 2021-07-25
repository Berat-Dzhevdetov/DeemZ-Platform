namespace DeemZ.Services.ResourceService
{
    using System.Collections.Generic;
    public interface IResourceService
    {
        IEnumerable<T> GetUserResources<T>(string uid);
    }
}