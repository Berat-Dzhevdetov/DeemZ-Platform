namespace DeemZ.Services.SurveyServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Survey;
    using DeemZ.Data.Models;
    using DeemZ.Global.Extensions;

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
            return permission && takenOrNot;
        }

        public T GetSurveyById<T>(string sid)
            => context.Surveys
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == sid)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .FirstOrDefault();

        public IEnumerable<T> GetUserCurrentCourseSurveys<T>(string uid, bool isPaid)
            => context.Surveys
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .Where(x => x.Course.UserCourses.Any(x => x.IsPaid == isPaid && x.UserId == uid)
                        && x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
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

        public string AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question)
        {
            var newlyQuestion = new SurveyQuestion
            {
                SurveyId = sid,
                Question = question.Question,
                IsOptional = question.IsOptional
            };

            context.SurveyQuestions.Add(newlyQuestion);

            context.SaveChanges();

            return newlyQuestion.Id;
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
            if (question.Answers.Any())
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

        public string DeleteAnswer(string said)
        {
            var answerToDelete = context.SurveyAnswers.FirstOrDefault(x => x.Id == said);

            var surveyQuestionId = answerToDelete.SurveyQuestionId;

            context.SurveyAnswers.Remove(answerToDelete);
            context.SaveChanges();

            return surveyQuestionId;
        }

        public bool CanUserAccessSurveyById(string sid, string uid)
            => context.Surveys
                .Any(x => !x.Users.Any(x => x.ApplicationUserId == uid) && x.Id == sid
                    && x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow
                    && x.Course.UserCourses.Any(x => x.IsPaid && x.UserId == uid));

        public void PrepareSurvey(string surveyId, out TakeSurveyFormModel survey)
        {
            survey = GetSurveyById<TakeSurveyFormModel>(surveyId);

            survey.Questions.ForEach(x =>
            {
                x.Answers = x.Answers.OrderByDescending(x => x.Text).ToList();
            });
        }

        public (Dictionary<string, string>, List<string>) ValidateSurvey(TakeSurveyFormModel survey)
        {
            var erros = new Dictionary<string, string>();
            var correctAnswerIds = new List<string>();

            var i = 0;
            var modelStateKey = "Questions[{0}]";
            var isGood = true;

            foreach (var question in survey.Questions)
            {
                isGood = true;

                var questionFromDb = context.SurveyQuestions.FirstOrDefault(x => x.Id == question.Id);

                if (questionFromDb == null)
                {
                    erros.Add(string.Format(modelStateKey, i), "Are you trying to hack me?");
                    isGood = false;
                }
                else if (question.Answers.Count == 0)
                {
                    erros.Add(string.Format(modelStateKey, i), "This question is not valid because it doesn't have any answers!");
                    isGood = false;
                }
                else if (!questionFromDb.IsOptional && question.Answers.Count(x => x.IsChosen) != 1)
                {
                    erros.Add(string.Format(modelStateKey, i), "The question should have exactly 1 answer checked.");
                    isGood = false;
                }
                else if (questionFromDb.IsOptional && question.Answers.Count(x => x.IsChosen) >= 2)
                {
                    erros.Add(string.Format(modelStateKey, i), "The question should have exactly 1 answer checked.");
                    isGood = false;
                }
                else if (question.Answers.Any(x => x.IsChosen)
                    && !IfAnswerExists(question.Answers.FirstOrDefault(x => x.IsChosen).Id))
                {
                    erros.Add(string.Format(modelStateKey, i), "Why did you change the answer id bro?");
                    isGood = false;
                }

                if (isGood && question.Answers.Any(x => x.IsChosen))
                    correctAnswerIds.Add(question.Answers.FirstOrDefault(x => x.IsChosen).Id);

                i++;
            }

            return (erros, correctAnswerIds);
        }

        private bool IfAnswerExists(string aid)
            => context.SurveyAnswers.Any(x => x.Id == aid);

        public void SaveSurvey(string sid, string uid, List<string> correctAnswerIds)
        {
            foreach (var answerId in correctAnswerIds)
            {
                var newUserSurveyAnswer = new ApplicationUserSurveyAnswer()
                {
                    ApplicationUserId = uid,
                    SurveyAnswerId = answerId
                };

                context.ApplicationUserSurveyAnswers.Add(newUserSurveyAnswer);
            }

            var applicationUserSurvey = new ApplicationUserSurvey()
            {
                ApplicationUserId = uid,
                SurveyId = sid
            };

            context.ApplicationUserSurveys.Add(applicationUserSurvey);

            context.SaveChanges();
        }

        public void AddRatingScaleToQuestion(string sqid)
        {
            var question = context.SurveyQuestions.FirstOrDefault(x => x.Id == sqid);

            for (int i = 2; i <= 6; i++)
            {
                var text = i.ToString();

                if (i == 2)
                    text += " - Poor";
                else if (i == 4)
                    text += " - Average";
                else if (i == 6)
                    text += " - Excellent";

                AddAnswerToQuestion(sqid, new AddSurveyAnswerFormModel
                {
                    Text = text
                });
            }
        }

        public IEnumerable<T> GetUserAllSurveys<T>(string uid, int page = 1)
            => context.Surveys
                .Where(x => x.Users.Any(x => x.ApplicationUserId == uid))
                .OrderByDescending(x => x.StartDate)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, 25)
                .ToList();

        public IDictionary<string, string> GetUserAnswers(string uid, string sid)
            => context.ApplicationUserSurveyAnswers
                .Include(x => x.SurveyAnswer)
                .ThenInclude(x => x.SurveyQuestion)
                .Where(x => x.ApplicationUserId == uid && x.SurveyAnswer.SurveyQuestion.SurveyId == sid)
                .ToDictionary(x => x.SurveyAnswer.SurveyQuestionId, x => x.SurveyAnswerId);

        public int GetUserAllSurveysCount(string userId)
            => context.ApplicationUserSurveys
                .Count(x => x.ApplicationUserId == userId);
    }
}