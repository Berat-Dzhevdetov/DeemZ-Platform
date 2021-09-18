namespace DeemZ.Global.WebConstants
{
    public static class Constant
    {
        public const string GlobalMessageKey = "GlobalMessage";
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm";
        public const string InformativeMessagesCacheKey = "InformativeMessagesCache";
        public const string AdminDashboradStatisticsCacheKey = "AdminDashboradStatisticsCache";

        public static class Role
        {
            public const string AdminRoleName = "Administrator";
            public const string LecturerRoleName = "Lecturer";
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
        }
    }
}