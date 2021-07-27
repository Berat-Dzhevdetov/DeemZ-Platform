namespace DeemZ.Services.CloudinaryServices
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using DeemZ.Data;

    public class FileServices : IFileServices
    {
        private Secret.CloudinarySetup cloudinarySetup;
        private Cloudinary cloudinary;
        private string tempFolder = Path.Combine("bin", "Debug", "net5.0", "temp");
        private string documentsFolder = Path.Combine("bin", "Debug", "net5.0", "documents");
        private string presentationsFolder = "presentations";
        private string wordsFolder = "word_files";
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
                Uri uriResult;
                bool result = Uri.TryCreate(path, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (result) return "url";
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
    }
}