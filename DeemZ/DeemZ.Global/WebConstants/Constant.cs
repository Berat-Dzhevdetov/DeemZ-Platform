namespace DeemZ.Global.WebConstants
{
    public static class Constant
    {
        public const string GlobalMessageKey = "GlobalMessage";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm";
        public const string InformativeMessagesCacheKey = "InformativeMessagesCache";
        public const string UpCommingCoursesCacheKey = "UpCommingCoursesCacheKey";
        public const string AdminDashboradStatisticsCacheKey = "AdminDashboradStatisticsCache";
        public const string CertificateTemplateCacheKey = "CertificateTemplateCache";

        public static class WebApi
        {
            public const string AccessForbiddenMessage = "ACCESS-FORBIDDEN";
        }

        public static class PromoCodes
        {
            public const string EmailSubject = "You have won a promo code!";
            public const string EmailContent = "Hello Dear <strong>{0}</strong>,<br> You have just won a promo code from us for {1}. The promo code is worth  <strong>${2}</strong>. You can use it when you sign up for a new course until <strong>{3}</strong>. The code is <strong>'{4}'</strong>";
        }

        public static class Role
        {
            public const string AdminRoleName = "Administrator";
            public const string LecturerRoleName = "Lecturer";
        }

        public static class EmailSender
        {
            public const string Email = "deemzonline@gmail.com";
            public const string Name = "DeemZ University";
        }

        public static class LayOut
        {
            public const string DefaultLayOut = "_Layout";
            public const string AdminLayout = "_AdminPanelLayout";
        }

        public static class AreaName
        {
            public const string AdminArea = "Admin";
            public const string ReportArea = "Report";
            public const string SurveyArea = "Survey";
            public const string InformativeArea = "Informative";
            public const string PromoCodeArea = "PromoCode";
        }
    }
}