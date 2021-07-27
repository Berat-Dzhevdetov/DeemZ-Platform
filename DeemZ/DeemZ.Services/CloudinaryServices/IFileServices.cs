namespace DeemZ.Services.CloudinaryServices
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IFileServices
    {
        private const int defaultSizeOfFile = 2097152; // 2 MB

        Task<ImageUploadResult> UploadImage(IFormFile file);
        bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile);
        bool CheckIfFileIsImage(IFormFile file);
        Task<string> UploadFile(IFormFile file, string path = null);

    }
}