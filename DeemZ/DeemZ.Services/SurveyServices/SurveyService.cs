namespace DeemZ.Services.SurveyServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Data.Models;

    public class SurveyService : ISurveyService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public SurveyService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool DoesTheUserHavePermissionToSurvey(string uid, string sid)
        {
            var permission = context.Courses
                .Any(x => x.UserCourses.Any(c => c.CourseId == x.Id && c.UserId == uid && c.IsPaid == true));
            var takenOrNot = context.ApplicationUserSurveys
                .Any(x => x.ApplicationUserId == uid && x.SurveyId == sid);
            return permission && !takenOrNot;
        }

        public T GetSurveyById<T>(string sid)
            => context.Surveys
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == sid)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        public IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid)
                => context.Surveys
                .Where(x => x.Course.UserCourses.Any(x => x.UserId == uid && x.IsPaid == true))
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetSurveysByCourseId<T>(string cid)
            => context.Surveys
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public void AddSurveyToCourse(string cid, AddSurveyFormModel survey)
        {
            var newlySurvey = new Survey
            {
                CourseId = cid,
                StartDate = survey.StartDate.ToUniversalTime(),
                EndDate = survey.EndDate.ToUniversalTime(),
                Name = survey.Name
            };

            context.Surveys.Add(newlySurvey);

            context.SaveChanges();
        }

        public string EditSurvey(string sid, EditSurveyFormModel survey)
        {
            var surveyToEdit = context.Surveys.FirstOrDefault(x => x.Id == sid);

            surveyToEdit.StartDate = survey.StartDate.ToUniversalTime();
            surveyToEdit.EndDate = survey.EndDate.ToUniversalTime();
            surveyToEdit.Name = survey.Name;

            context.SaveChanges();

            return surveyToEdit.CourseId;
        }

        public string DeleteSurvey(string sid)
        {
            var survey = GetSurveyById<Survey>(sid);

            foreach (var question in survey.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    context.SurveyAnswers.Remove(answer);
                }
                context.SurveyQuestions.Remove(question);
            }

            var courseId = survey.CourseId;

            context.Surveys.Remove(survey);

            context.SaveChanges();

            return courseId;
        }

        public IEnumerable<T> GetSurveyQuestions<T>(string sid)
            => context.SurveyQuestions
                .Where(x => x.SurveyId == sid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public void AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question)
        {
            var newlyQuestion = new SurveyQuestion
            {
                SurveyId = sid,
                Question = question.Question,
                IsOpenAnswer = question.IsOpenAnswer,
                IsOptional = question.IsOptional
            };

            context.SurveyQuestions.Add(newlyQuestion);

            context.SaveChanges();
        }

        public T GetQuestionById<T>(string sqid)
        {
            var question = context.SurveyQuestions.FirstOrDefault(x => x.Id == sqid);

            return mapper.Map<T>(question);
        }

        public string EditQuestion(string sqid, EditSurveyQuestionFormModel question)
        {
            var questionToEdit = context.SurveyQuestions.FirstOrDefault(x => x.Id == sqid);


            questionToEdit.Question = question.Question;
            questionToEdit.IsOptional = question.IsOptional;
            questionToEdit.IsOpenAnswer = question.IsOpenAnswer;

            DeleteQuestionAllAnswers(questionToEdit);

            context.SaveChanges();

            return questionToEdit.SurveyId;
        }

        public string DeleteQuestion(string sqid)
        {
            var questionToDel = context.SurveyQuestions.FirstOrDefault(x => x.Id == sqid);

            DeleteQuestionAllAnswers(questionToDel);

            var surveyId = questionToDel.SurveyId;

            context.SurveyQuestions.Remove(questionToDel);

            context.SaveChanges();

            return surveyId;
        }

        private void DeleteQuestionAllAnswers(SurveyQuestion question)
        {
            if (question.IsOpenAnswer && question.Answers.Any())
            {
                foreach (var answerToDel in question.Answers)
                {
                    context.SurveyAnswers.Remove(answerToDel);
                }
            }
        }

        public IEnumerable<T> GetAllAnswers<T>(string sqid)
            => context.SurveyAnswers
                .Where(x => x.SurveyQuestionId == sqid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public void AddAnswerToQuestion(string sqid, AddSurveyAnswerFormModel answer)
        {
            var newlyAnswer = new SurveyAnswer()
            {
                SurveyQuestionId = sqid,
                Text = answer.Text
            };

            context.SurveyAnswers.Add(newlyAnswer);

            context.SaveChanges();
        }

        public T GetAnswerById<T>(string said)
        {
            var answer = context.SurveyAnswers.FirstOrDefault(x => x.Id == said);

            return mapper.Map<T>(answer);
        }

        public string EditAnswer(string said, EditSurveyAnswerFormModel answer)
        {
            var answerEdit = context.SurveyAnswers.FirstOrDefault(x => x.Id == said);

            answerEdit.Text = answer.Text;

            context.SaveChanges();

            return answerEdit.SurveyQuestionId;
        }
    }
}
