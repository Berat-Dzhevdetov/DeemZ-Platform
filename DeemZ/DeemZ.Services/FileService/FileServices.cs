namespace DeemZ.Services.FileService
{
    using System;
    using System.IO;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using System.Net;
    using DeemZ.Data;
    using DeemZ.Global.Extensions;

    public class FileServices : IFileServices
    {
        private Secret.CloudinarySetup cloudinarySetup;
        private Cloudinary cloudinary;
        private string documentsFolder = "Documents";
        private string pdfsFiles = "mspdf";
        private string wordsFolder = "msword";
        private string videos = "Videos";
        private string images = "Images";
        private const int defaultSizeOfFile = 2097152; // 2 MB
        private readonly DeemZDbContext context;

        public FileServices(DeemZDbContext context)
        {
            cloudinarySetup = new Secret.CloudinarySetup();
            cloudinary = new Cloudinary(cloudinarySetup.Account);
            cloudinary.Api.Secure = true;
            this.context = context;
        }

        public bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile)
            => file.Length > (1024 * 1024 * mb);

        public bool CheckIfFileIsImage(IFormFile file)
            => FormFileExtensions.IsImage(file);

        private string GetFileExtension(IFormFile file)
            => Path.GetExtension(file.FileName).TrimStart('.');

        private string GetFileExtension(string file)
            => Path.GetExtension(file).TrimStart('.');

        public string PreparingFileForUploadAndUploadIt(IFormFile file, string path = null)
        {
            var newFileName = Guid.NewGuid().ToString();

            string folder = string.Empty;

            var extension = GetFileExtension(file);

            if (extension == "pdf" && path == "official_value")
                folder = $"{documentsFolder}/{pdfsFiles}/";
            else if ((extension == "doc" || extension == "docx") && path == "official_value")
                folder = $"{documentsFolder}/{wordsFolder}/";
            else if (extension == "mp4" && path == "official_value")
                folder = $"{videos}/";
            else if (CheckIfFileIsImage(file))
                folder = $"{images}/";
            else
                return null;

            return UploadFileToCloud(file, folder, newFileName, extension);
        }

        private string UploadFileToCloud(IFormFile file, string folder, string newFileName, string extension)
        {
            byte[] fileBytes;
            UploadResult uploadResult = null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            using (var memoryStream = new MemoryStream(fileBytes))
            {
                RawUploadParams uploadParams = new RawUploadParams
                {
                    Folder = folder,
                    File = new FileDescription(newFileName + "." + extension,memoryStream),
                    PublicId = newFileName
                };

                uploadResult = cloudinary.Upload(uploadParams);
            }

            return uploadResult?.SecureUrl.AbsoluteUri;
        }

        public void DeleteFile(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            
            cloudinary.Destroy(deletionParams);
        }

        public (byte[] fileContents, string contentType, string downloadName) GetFileBytesByResourceId(string rid)
        {
            var resource = context.Resources.Find(rid);

            var bytes = GetCloudFileAsBytes(resource.Path);

            var extension = GetFileExtension(resource.Path);

            var contentType = "";

            if (extension == "doc") contentType = "application/msword";
            else if (extension == "docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (extension == "pdf") contentType = "application/pdf";

            return (bytes, contentType, resource.Name + "." + extension);
        }

        private byte[] GetCloudFileAsBytes(string file)
        {
            var webClient = new WebClient();
            return webClient.DownloadData(file);
        }
    }
}