namespace DeemZ.Services.PdfServices
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Threading.Tasks;
    using iText.Html2pdf;
    using System.IO;
    using DeemZ.Data.Models;
    using DeemZ.Services.CourseServices;
    using DeemZ.Services.FileService;
    using DeemZ.Global.Extensions;
    using DeemZ.Data;
    using System.Linq;

    public class PdfService : IPdfService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICourseService courseService;
        private readonly IFileService fileService;
        private readonly DeemZDbContext context;
        private readonly Random rnd;

        public PdfService(UserManager<ApplicationUser> userManager, ICourseService courseService, IFileService fileService, DeemZDbContext context)
        {
            this.userManager = userManager;
            this.courseService = courseService;
            this.fileService = fileService;
            this.context = context;
            this.rnd = new Random();
        }

        public async Task<bool> GenerateCertificate(double grade, string cid, string uid, string serverLink)
        {
            grade = Math.Round(grade, 0);
            var user = await userManager.FindByIdAsync(uid);

            var studentName = $"{user.FirstName} {user.LastName}";
            var courseName = courseService.GetCourseById<Course>(cid).Name;

            var date = DateTime.Now.ToShortDateString();

            var id = Guid.NewGuid().ToString();

            var externalNumber = await Task.Run(() =>
            {
                int externalNumber = 1;
                do
                {
                    externalNumber = rnd.Next(int.MaxValue);
                }
                while (context.Certificates.Any(x => x.ExternalNumber == externalNumber));

                return externalNumber;
            });

            var certificate = new Certificate
            {
                CreatedOn = DateTime.UtcNow,
                Id = id,
                UserId = user.Id,
                ExternalNumber = externalNumber
            };

            var html = await File.ReadAllTextAsync("wwwroot/media/templates/template.html");
            html = html.Replace("{{STUDENT_NAME}}", studentName);
            html = html.Replace("{{COURSE_NAME}}", courseName);
            html = html.Replace("{{GRADE}}", grade.ToString());
            html = html.Replace("{{DATE}}", date);
            html = html.ReplaceAll("{{SERVER_LINK}}", serverLink);
            html = html.ReplaceAll("{{LINK}}", $"{serverLink}/User/ViewCertificate?id={externalNumber}");

            var memoryStream = new MemoryStream
            {
                Position = 0
            };

            HtmlConverter.ConvertToPdf(html, memoryStream);

            var (path, publicId) = fileService.UploadMemoryStreamToCloud(memoryStream.ToArray(), $"{studentName} - {courseName}", "Certificates", "pdf");

            certificate.Path = path;
            certificate.PublicId = publicId;

            context.Certificates.Add(certificate);
            await context.SaveChangesAsync();

            return true;
        }

    }
}