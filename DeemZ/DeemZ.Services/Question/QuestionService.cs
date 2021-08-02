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

            context.Questions.Add(newlyQuestion);
            context.SaveChanges();
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
    }
}