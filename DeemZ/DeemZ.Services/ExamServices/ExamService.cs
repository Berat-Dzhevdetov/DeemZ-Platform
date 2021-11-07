namespace DeemZ.Services.ExamServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Exam;
    using DeemZ.Data.Models;
    using DeemZ.Global.Extensions;

    public class ExamService : IExamService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ExamService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void CreateExam(string cid, AddExamFormModel exam)
        {
            var newlyExam = mapper.Map<Exam>(exam);
            newlyExam.CourseId = cid;

            newlyExam.StartDate = newlyExam.StartDate.ToUniversalTime();
            newlyExam.EndDate = newlyExam.EndDate.ToUniversalTime();

            context.Exams.Add(newlyExam);
            context.SaveChanges();
        }

        public bool DoesTheUserHavePermissionToExam(string uid, string eid)
        {
            var DoesTheUserHavePermissionToExam = context.Courses
                .Any(x => x.Exams.Any(x => x.Id == eid)
                && x.UserCourses.Any(x => x.IsPaid && x.UserId == uid));

            var doesUserHaveAlreadyTakeThisExam = context.ApplicationUserExams
                .Any(x => x.ApplicationUserId == uid && x.ExamId == eid);

            return DoesTheUserHavePermissionToExam && !doesUserHaveAlreadyTakeThisExam;
        }

        public string EditExam(string eid, AddExamFormModel exam)
        {
            var examToEdit = GetExamById<Exam>(eid);

            examToEdit.Name = exam.Name;
            examToEdit.IsPublic = exam.IsPublic;
            examToEdit.Password = exam.Password;
            examToEdit.ShuffleAnswers = exam.ShuffleAnswers;
            examToEdit.ShuffleQuestions = exam.ShuffleQuestions;
            examToEdit.StartDate = exam.StartDate.ToUniversalTime();
            examToEdit.EndDate = exam.EndDate.ToUniversalTime();

            context.SaveChanges();

            return eid;
        }

        public int EvaluateExam(TakeExamFormModel exam, string uid)
        {
            if (exam == null) return 0;

            var points = 0;

            foreach (var question in exam.Questions)
            {
                if (question.Answers == null || question.Answers.Count(x => x.IsChosen) >= 2 || question.Answers.Count(x => x.IsChosen) <= 0) continue;

                var userAnswerId = question.Answers.FirstOrDefault(x => x.IsChosen).Id;

                context.AnswerUsers.Add(new AnswerUsers
                {
                    AnswerId = userAnswerId,
                    UserId = uid
                });

                points += CalculatePoints(question.Id, userAnswerId);
            }

            context.SaveChanges();

            return points;
        }

        //Needs question Id and user's answer
        //checks if the answer is correct and returns the points
        private int CalculatePoints(string qid, string aid)
        {
            if (aid == null || qid == null) return 0;

            var correctAnswer = context.Answers
                 .Where(x => x.IsCorrect)
                 .Select(x => new
                 {
                     Id = x.Id,
                     Points = x.Question.Points,
                     QuestionId = x.QuestionId
                 })
                 .FirstOrDefault(x => x.QuestionId == qid);

            if (correctAnswer == null) return 0;

            return aid == correctAnswer.Id ? correctAnswer.Points : 0;
        }

        public bool GetExamById(string eid)
            => context.Exams.Any(x => x.Id == eid);

        public T GetExamById<T>(string eid)
        {
            var exam = context.Exams
                .Include(x => x.Course)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == eid);

            return mapper.Map<T>(exam);
        }

        public IEnumerable<T> GetExamsByCourseId<T>(string cid)
            => context.Exams
                .Include(x => x.Questions)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetExamsByUserId<T>(string uid)
            => context.Exams
                .Include(x => x.Course)
                .Include(x => x.Questions)
                .Include(e => e.Users.Where(x => x.ApplicationUserId == uid))
                .Include(e => e.Users.Where(x => x.ApplicationUserId == uid))
                .Where(e => e.Users.Any(x => x.ApplicationUserId == uid))
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public bool IsProvidedPasswordRight(string eid, string password)
            => context.Exams
                .Any(x => x.Password == password && x.Id == eid);

        public int SaveUserExam(string uid, int points, string eid)
        {
            var exam = GetExamById<Exam>(eid);

            exam.EndDate = exam.EndDate.ToLocalTime();

            if (DateTime.Now > exam.EndDate) return -1;

            var maxExamPoints = exam.Questions.Sum(x => x.Points);

            var maxCredits = exam.Course.Credits;

            var earnedPointsInPercentages = points / (maxExamPoints * 1.0);
            var creditsEarned = (int)Math.Round(maxCredits * earnedPointsInPercentages);

            var userExam = new ApplicationUserExam()
            {
                ApplicationUserId = uid,
                EarnedPoints = points,
                ExamId = eid,
                EarnedCredits = creditsEarned,
                HandOverOn = DateTime.UtcNow
            };

            context.ApplicationUserExams.Add(userExam);
            context.SaveChanges();

            return maxExamPoints;
        }

        public string GetCourseIdByExamId(string eid)
            => context.Exams.FirstOrDefault(x => x.Id == eid).CourseId;

        public IDictionary<string, string> GetUserExamAnswers(string eid, string uid)
            => context.AnswerUsers
                .Include(x => x.Answer)
                .ThenInclude(x => x.Question)
                .Where(x => x.Answer.Question.ExamId == eid && x.UserId == uid)
                .ToDictionary(x => x.Answer.QuestionId, x => x.AnswerId);

        public IDictionary<string, string> GetExamsAsKeyValuePair(DateTime prevDate)
            => context.Exams
                .Where(x => x.EndDate > prevDate)
                .ToDictionary(x => x.Id, x => x.Name);

        public IEnumerable<T> GetUserExams<T>(int page = 1, int quantity = 20)
            => context.ApplicationUserExams
                .Where(x => x.HandOverOn > DateTime.UtcNow.AddDays(-30))
                .OrderByDescending(x => x.EarnedPoints)
                .ThenBy(x => x.HandOverOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public IEnumerable<T> GetUserExams<T>(string eid, int page = 1, int quantity = 20)
        => context.ApplicationUserExams
                .Where(x => x.HandOverOn > DateTime.UtcNow.AddDays(-30) && x.ExamId == eid)
                .OrderByDescending(x => x.EarnedPoints)
                .ThenBy(x => x.HandOverOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();
    }
}