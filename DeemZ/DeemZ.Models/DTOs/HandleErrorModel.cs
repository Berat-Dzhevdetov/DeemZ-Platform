namespace DeemZ.Models.DTOs
{
    public class HandleErrorModel
    {
        public string Message { get; init; }
        public HttpStatusCodes StatusCode { get; init; }
    }
}