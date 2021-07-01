﻿namespace DeemZ.Data
{
    public class DataConstants
    {
        public const int DefaultIdLength = 40;
        public class User
        {
            public const int MinUsernameLength = 4; 
            public const int MaxUsernameLength = 16;
            public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$";
            public const int MinLengthForNames = 3;
            public const int MaxLengthForNames = 25;
        }

        public class Forum
        {
            public const int MinTitleLength = 10;
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
            public const decimal DefaultPrice = 220m;
        }

        public class Lecture
        {
            public const int MinNameLength = 5;
        }

        public class Description
        {
            public const int MinNameLength = 3;
        }

        public class Exam
        {
            public const int MinNameLength = 5;
        }

        public class Question
        {
            public const int MinTextLength = 10;
        }

        public class Answer
        {
            public const int MinAnswerLength = 5;
        }
    }
}
