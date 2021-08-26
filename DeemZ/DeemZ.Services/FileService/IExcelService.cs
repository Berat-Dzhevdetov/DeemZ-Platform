namespace DeemZ.Services.FileService
{
    public interface IExcelService
    {
        byte[] ReturnAsBytes();
        byte[] ExportExam(string examId);
    }
}