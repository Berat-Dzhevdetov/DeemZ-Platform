namespace DeemZ.Data
{
    public class DataConstants
    {
        public const int DefaultIdLength = 40;
        public class User
        {
            public const int MinUsernameLength = 4; 
            public const int MaxUsernameLength = 16;
            public const int MinLengthForNames = 3;
            public const int MaxLengthForNames = 25;
            public const string UsernameRegex = @"^[a-zA-Z]([._]?[a-zA-Z0-9]+)+$";
            public const string DefaultProfilePictureUrl = "/media/anonymous-picture.jpg";
        }

        public class Forum
        {
            public const int MinTitleLength = 5;
            public const int MaxTitleLength = 30;
            public const int MinDescriptionLength = 50;
        }

        public class Comment
        {
            public const int MinTextLength = 10;
        }

        public class Report
        {
            public const int MinDescriptionLength = 20;
        }

        public class Course
        {
            public const int MinNameLength = 5;
            public const int MaxNameLength = 100;
            public const decimal DefaultPrice = 220m;
            public const int MinCredits = 1;
            public const int MaxCredits = 15;
            public const decimal MinMoney = 50m;
            public const decimal MaxMoney = 440m;
            public const int MinDescriptionLength = 20;
            public const int MaxDescriptionLength = 20000;
        }

        public class Lecture
        {
            public const int MinNameLength = 5;
            public const int MaxNameLength = 100;

        }

        public class Description
        {
            public const int MinNameLength = 3;
            public const int MaxNameLength = 40;
        }

        public class Exam
        {
            public const int MinNameLength = 5;
            public const int MaxNameLength = 100;
            public const int MinPoints = 10;
            public const int MinPasswordLength = 5;
            public const int MaxPasswordLength = 15;
            public const int AtLeastCountQuestions = 5;
        }

        public class Question
        {
            public const int MinTextLength = 10;
            public const int MaxTextLength = 100;
            public const int MinPoints = 1;
            public const int MaxPoints = 5;
            public const int AtLeastCountAnswers = 2;
        }

        public class Answer
        {
            public const int MinTextLength = 3;
            public const int MaxTextLength = 100;
        }

        public class Resource
        {
            public const int MinNameLength = 3;
            public const int MaxNameLength = 100;
            public const int PathMinLength = 10; 
            public const int MaxIconLength = 50;
        }

        public class Survey
        {
            public const int MinNameLength = 5;
            public const int MaxNameLength = 150;

        }

        public class SurveyQuestion
        {
            public const int MinNameLength = 5;
            public const int MaxQuestionLength = 100;
        }

        public class SurveyAnswer
        {
            public const int MaxTextLength = 50;
        }

        public class InformativeMessages
        {
            public const int MaxTitleLength = 100;
            public const int MinTitleLength = 3;
            public const int MaxDescriptionLength = 100000;
            public const int MinDescriptionLength = 20;
        }
    }
}