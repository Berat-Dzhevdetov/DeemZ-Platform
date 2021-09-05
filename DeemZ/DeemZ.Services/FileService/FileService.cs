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
    using DeemZ.Infrastructure;

    public class FileService : IFileService
    {
        private Secret.CloudinarySetup cloudinarySetup;
        private Cloudinary cloudinary;
        private string documentsFolder = "Documents";
        private string pdfsFolder = "mspptx";
        private string wordsFolder = "msword";
        private string videosFolder = "Videos";
        private string imagesFolder = "Images";
        private const int defaultSizeOfFile = 2; // MB
        private readonly DeemZDbContext context;

        public FileService(DeemZDbContext context)
        {
            cloudinarySetup = new Secret.CloudinarySetup();
            cloudinary = new Cloudinary(cloudinarySetup.Account);
            cloudinary.Api.Secure = true;
            this.context = context;
        }

        //if file is above given mbs will return true
        //otherwise false
        public bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile)
            => file.Length > (1024 * 1024 * mb);

        public bool CheckIfFileIsImage(IFormFile file)
            => FormFileExtensions.IsImage(file);

        private string GetFileExtension(IFormFile file)
            => Path.GetExtension(file.FileName).TrimStart('.');

        private string GetFileExtension(string file)
            => Path.GetExtension(file).TrimStart('.');

        public (string url, string publicId) PreparingFileForUploadAndUploadIt(IFormFile file, string path = null)
        {
            var newFileName = Guid.NewGuid().ToString();

            string folder = string.Empty;

            var isExtensionNeeded = false;

            var extension = GetFileExtension(file);

            if (extension == "pptx" && path == "official_value")
            {
                folder = $"{documentsFolder}/{pdfsFolder}/";
                isExtensionNeeded = true;
            }
            else if ((extension == "doc" || extension == "docx") && path == "official_value")
            {
                folder = $"{documentsFolder}/{wordsFolder}/";
                isExtensionNeeded = true;
            }
            else if (extension == "mp4" && path == "official_value")
            {
                folder = $"{videosFolder}/";
            }
            else if (CheckIfFileIsImage(file))
            {
                folder = $"{imagesFolder}/";
            }
            else
            {
                return (null, null);
            }

            var publicId = $"{folder}{newFileName}";

            publicId += isExtensionNeeded ? extension : string.Empty;

            return (UploadFileToCloud(file, folder, newFileName, extension), publicId);
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
                    File = new FileDescription(newFileName + "." + extension, memoryStream),
                    PublicId = newFileName
                };

                uploadResult = cloudinary.Upload(uploadParams);
            }

            return uploadResult?.SecureUrl.AbsoluteUri;
        }

        public void DeleteFile(string publicId, bool isImg = false, bool isVideo = false)
        {
            var deletionParams = new DeletionParams(publicId);

            if (isImg) deletionParams.ResourceType = ResourceType.Image;
            else if (isVideo) deletionParams.ResourceType = ResourceType.Video;
            else deletionParams.ResourceType = ResourceType.Raw;

            var results = cloudinary.Destroy(deletionParams);
        }

        public (byte[] fileContents, string contentType, string downloadName) GetFileBytesByResourceId(string rid)
        {
            var resource = context.Resources.Find(rid);

            var bytes = GetCloudFileAsBytes(resource.Path);

            var extension = GetFileExtension(resource.Path);

            var contentType = "";

            if (extension == "doc") contentType = "application/msword";
            else if (extension == "docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (extension == "pptx") contentType = "	application/vnd.openxmlformats-officedocument.presentationml.presentation";

            return (bytes, contentType, resource.Name + "." + extension);
        }

        private byte[] GetCloudFileAsBytes(string file)
        {
            var webClient = new WebClient();
            return webClient.DownloadData(file);
        }
    }
}