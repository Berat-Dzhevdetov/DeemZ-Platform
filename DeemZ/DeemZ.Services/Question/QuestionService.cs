namespace DeemZ.Services.Question
{
    using AutoMapper;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Exam;

    public class QuestionService : IQuestionService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public QuestionService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void AddQuestionToExam(string examId, AddQuestionFormModel question)
        {
            var newlyQuestion = mapper.Map<Question>(question);
            newlyQuestion.ExamId = examId;
            newlyQuestion.Answers = newlyQuestion.Answers.Where(x => x.Text != null).ToList();

            context.Questions.Add(newlyQuestion);
            context.SaveChanges();
        }

        public string Delete(string qid)
        {
            var question = GetQuestionById<Question>(qid);

            foreach (var answer in question.Answers)
            {
                DeleteAnswer(answer.Id);
            }

            context.Questions.Remove(question);
            context.SaveChanges();

            return question.ExamId;
        }

        private T GetAnswerById<T>(string aid)
        {
            var answer = context.Answers.FirstOrDefault(x => x.Id == aid);

            return mapper.Map<T>(answer);
        }

        private void DeleteAnswer(string aid)
        {
            var answerToDel = GetAnswerById<Answer>(aid);

            context.Answers.Remove(answerToDel);
            context.SaveChanges();
        }

        public bool GetQuestionById(string qid)
            => context.Questions.Any(x => x.Id == qid);

        public T GetQuestionById<T>(string qid)
        {
            var question = context.Questions
                .Include(x => x.Answers)
                .FirstOrDefault(x => x.Id == qid);

            return mapper.Map<T>(question);
        }

        public IEnumerable<T> GetQuestionsByExamId<T>(string examId)
            => context.Questions
                .Include(x => x.Exam)
                .Where(x => x.ExamId == examId)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public string ValidateQuestionFormModel(AddQuestionFormModel question)
        {
            var tempList = question.Answers.Where(x => x.Text != null).ToList();

            if (tempList.Count < DataConstants.Question.AtLeastCountAnswers)
                return $"Question should have at leats {DataConstants.Question.AtLeastCountAnswers} answers";

            if(tempList.Count(x => x.IsCorrect) != 1)
                return $"Question should have one correct answer";

            return null;
        }

        public string Edit(string questionId, AddQuestionFormModel question)
        {
            var questionToEdit = GetQuestionById<Question>(questionId);
            var examId = questionToEdit.ExamId;

            context.Questions.Attach(questionToEdit);

            questionToEdit.Text = question.Text;
            questionToEdit.Points = question.Points;

            questionToEdit.Answers = questionToEdit.Answers.Where(x => x.Text != null).ToList();

            context.SaveChanges();

            return examId;
        }
    }
}