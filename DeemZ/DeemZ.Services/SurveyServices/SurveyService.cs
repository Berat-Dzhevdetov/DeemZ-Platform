namespace DeemZ.Services.SurveyServices
{
    using AutoMapper;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using System.Threading.Tasks;
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
                .Any(x => x.UserCourses.Any(c => c.CourseId == x.Id && c.UserId == uid && c.IsPaid));
            var takenOrNot = context.ApplicationUserSurveys
                .Any(x => x.ApplicationUserId == uid && x.SurveyId == sid);
            return permission && !takenOrNot;
        }

        public async Task<T> GetSurveyById<T>(string sid)
            => await context.Surveys
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == sid)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetUserCurrentCourseSurveys<T>(string uid, bool isPaid) 
            =>  await context.Surveys
                .Include(x => x.Course)
                .ThenInclude(x => x.UserCourses)
                .Where(x => x.Course.UserCourses.Any(x => x.IsPaid == isPaid && x.UserId == uid)
                        && x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<T>> GetSurveysByCourseId<T>(string cid)
            => await context.Surveys
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task AddSurveyToCourse(string cid, AddSurveyFormModel survey)
        {
            var newlySurvey = new Survey
            {
                CourseId = cid,
                StartDate = survey.StartDate.ToUniversalTime(),
                EndDate = survey.EndDate.ToUniversalTime(),
                Name = survey.Name
            };

            context.Surveys.Add(newlySurvey);

            await context.SaveChangesAsync();
        }

        public async Task<string> EditSurvey(string sid, EditSurveyFormModel survey)
        {
            var surveyToEdit = context.Surveys.FirstOrDefault(x => x.Id == sid);

            surveyToEdit.StartDate = survey.StartDate.ToUniversalTime();
            surveyToEdit.EndDate = survey.EndDate.ToUniversalTime();
            surveyToEdit.Name = survey.Name;

            await context.SaveChangesAsync();

            return surveyToEdit.CourseId;
        }

        public async Task<string> DeleteSurvey(string sid)
        {
            var survey = await GetSurveyById<Survey>(sid);

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

            await context.SaveChangesAsync();

            return courseId;
        }

        public async Task<IEnumerable<T>> GetSurveyQuestions<T>(string sid)
            => await context.SurveyQuestions
                .Where(x => x.SurveyId == sid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<string> AddQuestionToSurvey(string sid, AddSurveyQuestionFormModel question)
        {
            var newlyQuestion = new SurveyQuestion
            {
                SurveyId = sid,
                Question = question.Question,
                IsOptional = question.IsOptional
            };

            context.SurveyQuestions.Add(newlyQuestion);

            await context.SaveChangesAsync();

            return newlyQuestion.Id;
        }

        public async Task<T> GetQuestionById<T>(string sqid)
        {
            var question = await context.SurveyQuestions.FirstOrDefaultAsync(x => x.Id == sqid);

            return mapper.Map<T>(question);
        }

        public async Task<string> EditQuestion(string sqid, EditSurveyQuestionFormModel question)
        {
            var questionToEdit = await context.SurveyQuestions.FirstOrDefaultAsync(x => x.Id == sqid);


            questionToEdit.Question = question.Question;
            questionToEdit.IsOptional = question.IsOptional;

            DeleteQuestionAllAnswers(questionToEdit);

            await context.SaveChangesAsync();

            return questionToEdit.SurveyId;
        }

        public async Task<string> DeleteQuestion(string sqid)
        {
            var questionToDel = await context.SurveyQuestions.FirstOrDefaultAsync(x => x.Id == sqid);

            DeleteQuestionAllAnswers(questionToDel);

            var surveyId = questionToDel.SurveyId;

            context.SurveyQuestions.Remove(questionToDel);

            await context.SaveChangesAsync();

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

        public async Task<IEnumerable<T>> GetAllAnswers<T>(string sqid)
            => await context.SurveyAnswers
                .Where(x => x.SurveyQuestionId == sqid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task AddAnswerToQuestion(string sqid, AddSurveyAnswerFormModel answer)
        {
            var newlyAnswer = new SurveyAnswer()
            {
                SurveyQuestionId = sqid,
                Text = answer.Text
            };

            context.SurveyAnswers.Add(newlyAnswer);

            await context.SaveChangesAsync();
        }

        public async Task<T> GetAnswerById<T>(string said)
        {
            var answer = await context.SurveyAnswers.FirstOrDefaultAsync(x => x.Id == said);

            return mapper.Map<T>(answer);
        }

        public async Task<string> EditAnswer(string said, EditSurveyAnswerFormModel answer)
        {
            var answerEdit = await context.SurveyAnswers.FirstOrDefaultAsync(x => x.Id == said);

            answerEdit.Text = answer.Text;

            await context.SaveChangesAsync();

            return answerEdit.SurveyQuestionId;
        }

        public async Task<string> DeleteAnswer(string said)
        {
            var answerToDelete = await context.SurveyAnswers.FirstOrDefaultAsync(x => x.Id == said);

            var surveyQuestionId = answerToDelete.SurveyQuestionId;

            context.SurveyAnswers.Remove(answerToDelete);
            await context.SaveChangesAsync();

            return surveyQuestionId;
        }

        public bool CanUserAccessSurveyById(string sid, string uid)
            => context.Surveys
                .Any(x => !x.Users.Any(x => x.ApplicationUserId == uid) && x.Id == sid
                    && x.StartDate <= DateTime.UtcNow && x.EndDate > DateTime.UtcNow
                    && x.Course.UserCourses.Any(x => x.IsPaid && x.UserId == uid));

        public void PrepareSurvey(string surveyId, out TakeSurveyFormModel survey)
        {
            survey = Task.Run(async () => await GetSurveyById<TakeSurveyFormModel>(surveyId)).Result; 

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

        public async Task SaveSurvey(string sid, string uid, List<string> correctAnswerIds)
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

            await context.SaveChangesAsync();
        }

        public async Task AddRatingScaleToQuestion(string sqid)
        {
            var question = await context.SurveyQuestions.FirstOrDefaultAsync(x => x.Id == sqid);

            for (int i = 2; i <= 6; i++)
            {
                var text = i.ToString();

                if (i == 2)
                    text += " - Poor";
                else if (i == 4)
                    text += " - Average";
                else if (i == 6)
                    text += " - Excellent";

                await AddAnswerToQuestion(sqid, new AddSurveyAnswerFormModel
                {
                    Text = text
                });
            }
        }

        public IEnumerable<T> GetUserAllSurveys<T>(string uid, int page = 1, int quantity = 25)
            => context.Surveys
                .Where(x => x.Users.Any(x => x.ApplicationUserId == uid))
                .OrderByDescending(x => x.StartDate)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .Paging(page, quantity)
                .ToList();

        public async Task<IDictionary<string, string>> GetUserAnswers(string uid, string sid)
            => await context.ApplicationUserSurveyAnswers
                .Include(x => x.SurveyAnswer)
                .ThenInclude(x => x.SurveyQuestion)
                .Where(x => x.ApplicationUserId == uid && x.SurveyAnswer.SurveyQuestion.SurveyId == sid)
                .ToDictionaryAsync(x => x.SurveyAnswer.SurveyQuestionId, x => x.SurveyAnswerId);

        public async Task<int> GetUserAllSurveysCount(string userId)
            => await context.ApplicationUserSurveys
                .CountAsync(x => x.ApplicationUserId == userId);
    }
}