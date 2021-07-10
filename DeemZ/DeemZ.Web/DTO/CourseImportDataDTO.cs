namespace DeemZ.Web.DTO
{
    using System.Collections.Generic;

    public class CourseImportDataDTO
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SignUpStartDate { get; set; }
        public string SignUpEndDate { get; set; }

        public List<LectureImportDataDTO> Lectures { get; set; }
    }
    
    public class LectureImportDataDTO
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public List<string> Descriptions { get; set; }
        public List<ResourcesImportDataDTO> Resources { get; set; }
    }

    public class ResourcesImportDataDTO
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ResourceType { get; set; }

    }
}
