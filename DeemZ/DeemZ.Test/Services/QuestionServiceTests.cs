﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeemZ.Data;
using DeemZ.Data.Models;
using DeemZ.Models.FormModels.Exam;
using Xunit;

namespace DeemZ.Test.Services
{
    public class QuestionServiceTests : BaseTestClass
    {
        public QuestionServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public async Task AddingQuestionToExamShouldCorrectlyAddIt()
        {
            var expectedQuestions = 1;

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            await questionService.AddQuestionToExam(examId, new AddQuestionFormModel()
            {
                Text = "Test Question",
                Points = 10,
                Answers = new List<AddAnswerFormModel>()
                {
                    new AddAnswerFormModel()
                    {
                        IsCorrect = false,
                        Text = "Test-Answer"
                    }
                }
            });

            var actualQuestions = examService.GetExamById<TakeExamFormModel>(examId).Questions.Count;

            Assert.Equal(expectedQuestions, actualQuestions);
        }

        [Fact]
        public async Task DeletingQuestionShouldRemoveItFromExam()
        {
            var expectedQuestions = 0;

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestionsAnswers(examId);
            var questionId = context.Questions.First().Id;

            await questionService.Delete(questionId);

            var actualQuestions = examService.GetExamById<TakeExamFormModel>(examId).Questions.Count;

            Assert.Equal(expectedQuestions, actualQuestions);
        }

        [Fact]
        public async Task GettingQuestionByIdShouldReturnTheCorrectQuestion()
        {
            var expectedQuestionText = "Are you cool?";

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestions(examId);
            var questionId = context.Questions.First().Id;

            var actualQuestionText = questionService.GetQuestionById<Question>(questionId).Text;

            Assert.Equal(expectedQuestionText, actualQuestionText);
        }

        [Fact]
        public async Task GettingQuestionByIdShouldReturnTrueIfTheQuestionExists()
        {
            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestions(examId);
            var questionId = context.Questions.First().Id;

            var doesQuestionExist = questionService.GetQuestionById(questionId);

            Assert.True(doesQuestionExist);
        }

        [Fact]
        public void GettingQuestionByIdShouldReturnFalseIfTheQuestionDoesNotExists() => Assert.False(questionService.GetQuestionById("invalid-question-id"));

        [Fact]
        public async Task GettingQuestionsByExamIdShouldReturnTheCorrectQuestions()
        {
            var expectedQuestionsCount = 1;

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestions(examId);

            var actualQuestionsCount = questionService.GetQuestionsByExamId<TakeExamQuestionFormModel>(examId).Count();

            Assert.Equal(expectedQuestionsCount, actualQuestionsCount);
        }

        [Fact]
        public async Task EditingQuestionShouldChangeItsProperties()
        {
            var expectedQuestionPoints = 69;

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestions(examId);
            var questionId = context.Questions.First().Id;

            await questionService.Edit(questionId,
                new AddQuestionFormModel()
                {
                    Points = expectedQuestionPoints,
                }
            );

            var question = questionService.GetQuestionById<Question>(questionId);

            Assert.Equal(expectedQuestionPoints, question.Points);
        }

        [Fact]
        public async Task EditingQuestionShouldChangeTheAnswersOnlyIfTheirTextIsNotNull()
        {
            var expectedAnswersCount = 1;

            var courseId = await SeedCourse();

            var examId = SeedExam(courseId);

            SeedExamQuestionsAnswers(examId);

            var questionId = context.Questions.First().Id;

            context.Answers.Add(new Answer()
            {
                Text = null,
                IsCorrect = true,
                QuestionId = questionId
            });
            context.SaveChanges();

            await questionService.Edit(questionId,
                new AddQuestionFormModel()
                {
                    Points = 69
                }
            );

            var actualAnswersCount = questionService.GetQuestionById<Question>(questionId).Answers.Count;

            Assert.Equal(expectedAnswersCount, actualAnswersCount);
        }

        [Fact]
        public void ValidatingAQuestionWithLessThanRequiredAnswersShouldReturnErrorMessage()
        {
            var expectedResult = $"Question should have at leats {DataConstants.Question.AtLeastCountAnswers} answers";

            var actualResult = questionService.ValidateQuestionFormModel(new AddQuestionFormModel()
            {
                Text = "Test Question",
                Points = 10,
                Answers = new List<AddAnswerFormModel>()
                {
                    new AddAnswerFormModel()
                    {
                        IsCorrect = true,
                        Text = "Test-Answer"
                    }
                }
            });

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ValidatingAQuestionWithNoCorrectAnswersAnswersShouldReturnErrorMessage()
        {
            var expectedResult = $"Question should have one correct answer";

            var actualResult = questionService.ValidateQuestionFormModel(new AddQuestionFormModel()
            {
                Text = "Test Question",
                Points = 10,
                Answers = new List<AddAnswerFormModel>()
                {
                    new AddAnswerFormModel()
                    {
                        IsCorrect = false,
                        Text = "Incorrect-Test-Answer-1"
                    },
                    new AddAnswerFormModel()
                    {
                        IsCorrect = false,
                        Text = "Incorrect-Test-Answer-2"
                    }
                }
            });

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ValidatingAQuestionWithMoreThanOneCorrectAnswersAnswersShouldReturnErrorMessage()
        {
            var expectedResult = $"Question should have one correct answer";

            var actualResult = questionService.ValidateQuestionFormModel(new AddQuestionFormModel()
            {
                Text = "Test Question",
                Points = 10,
                Answers = new List<AddAnswerFormModel>()
                {
                    new AddAnswerFormModel()
                    {
                        IsCorrect = true,
                        Text = "Correct-Test-Answer-1"
                    },
                    new AddAnswerFormModel()
                    {
                        IsCorrect = true,
                        Text = "Correct-Test-Answer-2"
                    }
                }
            });

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void ValidatingAQuestionWithShouldReturnNullIfEverythingIsOK()
        {
            string expectedResult = null;

            var actualResult = questionService.ValidateQuestionFormModel(new AddQuestionFormModel()
            {
                Text = "Test Question",
                Points = 10,
                Answers = new List<AddAnswerFormModel>()
                {
                    new AddAnswerFormModel()
                    {
                        IsCorrect = true,
                        Text = "Correct-Test-Answer-1"
                    },
                    new AddAnswerFormModel()
                    {
                        IsCorrect = false,
                        Text = "Incorrect-Test-Answer-2"
                    }
                }
            });

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
