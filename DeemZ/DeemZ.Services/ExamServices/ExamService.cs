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

            examToEdit.StartDate = exam.StartDate.ToUniversalTime();
            examToEdit.EndDate = exam.EndDate.ToUniversalTime();

            examToEdit = mapper.Map<Exam>(exam);

            context.SaveChanges();

            return context.Exams.Where(x => x.Id == eid).Select(x => x.CourseId).FirstOrDefault();
        }

        public int EvaluateExam(TakeExamFormModel exam)
        {
            if (exam == null) return 0;

            var points = 0;

            foreach (var question in exam.Questions)
            {
                if (question.Answers == null || question.Answers.Count(x => x.IsChosen) >= 2 || question.Answers.Count(x => x.IsChosen) <= 0) continue;

                points += CalculatePoints(question.Id, question.Answers.FirstOrDefault(x => x.IsChosen).Id);
            }

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

        public bool IsProvidedPasswordRight(string eid, string password)
            => context.Exams
                .Any(x => x.Password == password && x.Id == eid);

        public int SaveUserExam(string uid, int points, string eid)
        {
            var exam = GetExamById<Exam>(eid);

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
                EarnedCredits = creditsEarned
            };

            context.ApplicationUserExams.Add(userExam);
            context.SaveChanges();

            return maxExamPoints;
        }
    }
}