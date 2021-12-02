namespace DeemZ.Services.FileService
{
    using Microsoft.AspNetCore.Http;
    using System.IO;

    public interface IFileService
    {
        private const int defaultSizeOfFile = 2; // MB
        bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile);
        bool CheckIfFileIsImage(IFormFile file);
        (string url, string publicId) PreparingFileForUploadAndUploadIt(IFormFile file, string path = null);
        void DeleteFile(string publicId, bool isImg = false, bool isVideo = false);
        (byte[] fileContents, string contentType, string downloadName) GetFileBytesByResourceId(string rid);
        (string url, string publicId) UploadFileBytesToCloud(byte[] fileBytes, string newFileName);
        (string url, string publicId) UploadMemoryStreamToCloud(byte[] bytes, string newFileName, string folderName, string extension);
    }
}