namespace DeemZ.Services.FileService
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using DeemZ.Services.ResourceService;
    using DeemZ.Models.ServiceModels.Resource;

    public class FileServices : IFileServices
    {
        private Secret.CloudinarySetup cloudinarySetup;
        private Cloudinary cloudinary;
        private string tempFolder = Path.Combine("bin", "Debug", "net5.0", "temp");
        private string documentsFolder = Path.Combine("wwwroot", "documents");
        private string presentationsFolder = "presentations";
        private string wordsFolder = "word_files";
        private const int defaultSizeOfFile = 2097152; // 2 MB
        private readonly IResourceService resourceService;


        public FileServices(IResourceService resourceService)
        {
            cloudinarySetup = new Secret.CloudinarySetup();
            cloudinary = new Cloudinary(cloudinarySetup.Account);
            cloudinary.Api.Secure = true;
            this.resourceService = resourceService;
        }

        public bool CheckIfFileIsUnderMB(IFormFile file, int mb = defaultSizeOfFile)
            => file.Length > (1024 * 1024 * mb);

        public bool CheckIfFileIsImage(IFormFile file)
            => FormFileExtensions.IsImage(file);

        private string GetFileExtension(IFormFile file)
            => Path.GetExtension(file.FileName).TrimStart('.');

        private string GetFileExtension(string file)
            => Path.GetExtension(file).TrimStart('.');

        public bool IsAllowedFile(IFormFile file)
            => FormFileExtensions.IsPdf(file.FileName) || FormFileExtensions.IsWord(file.FileName);

        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            //Uploading the file to the system file
            var path = await UploadFile(file);

            //Uploading the file to the cloud
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path)
            };

            //Deleting the file from the file system
            DeleteFile(path);

            return cloudinary.Upload(uploadParams);
        }

        //Uploads file to the file system
        public async Task<string> UploadFile(IFormFile file, string path = null)
        {
            if (!(path == "official_value" && IsAllowedFile(file)))
            {
                if (IsUrl(path)) return "url";
                return null;
            }

            var extension = GetFileExtension(file);

            //Create a unique name so that there is no problem with the file system
            var fileName = Guid.NewGuid().ToString() + "." + extension;

            if (extension == "pdf" && path == "official_value") path = Path.Combine(documentsFolder, presentationsFolder);
            else if ((extension == "doc" || extension == "docx") && path == "official_value") path = Path.Combine(documentsFolder, wordsFolder);
            else if (path == "oficial_value" && CheckIfFileIsImage(file)) path = tempFolder;

            path = Path.Combine(path, fileName); 

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return path;
        }

        //Deletes file from the file system
        private void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public (byte[] fileContents, string contentType,string downloadName) GetFileBytesByResourceId(string rid)
        {
            var resource = resourceService.GetResourceById<ResourceServiceModel>(rid);

            if (IsUrl(resource.Path)) return (new byte[0], null, resource.Name);

            var bytes = GetFileAsBytes(resource.Path);

            var extension = GetFileExtension(resource.Path);

            var contentType = "";

            if (extension == "doc") contentType = "application/msword";
            else if (extension == "docx") contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (extension == "pdf") contentType = "application/pdf";

            return (bytes, contentType,resource.Name + "." + extension);
        }

        private byte[] GetFileAsBytes(string file)
        {
            var path = documentsFolder;

            var fileName = Path.GetFileName(file);

            var extension = GetFileExtension(file);

            if (extension == "doc" || extension == "docx") path = Path.Combine(path, wordsFolder);
            else if (extension == "pdf") path = Path.Combine(path, presentationsFolder);

            path = Path.Combine(path, fileName);

            return File.ReadAllBytes(path);
        }

        private bool IsUrl(string path)
        {
            Uri uriResult;
            return Uri.TryCreate(path, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}