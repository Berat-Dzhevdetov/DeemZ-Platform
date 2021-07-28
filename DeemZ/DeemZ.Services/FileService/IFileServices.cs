﻿namespace DeemZ.Services.FileService
{
    using Microsoft.AspNetCore.Http;

    public interface IFileServices
    {
        private const int defaultSizeOfFile = 2097152; // 2 MB

        bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile);
        bool CheckIfFileIsImage(IFormFile file);
        string PreparingFileForUploadAndUploadIt(IFormFile file, string path = null);
        void DeleteFile(string publicId);
        (byte[] fileContents, string contentType, string downloadName) GetFileBytesByResourceId(string rid);
    }
}