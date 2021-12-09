using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeemZ.Data.Models;
using DeemZ.Models.FormModels.Exam;
using DeemZ.Models.ViewModels.Exams;
using Xunit;

namespace DeemZ.Test.Services
{
    public class ExamServiceTests : BaseTestClass
    {
        public ExamServiceTests()
        {
            SetUpServices();
        }

        [Fact]
        public async Task CreatingExamShouldAddItToTheDatabase()
        {
            var expectedExamsCount = 1;

            var courseId = await SeedCourse();

            await examService.CreateExam(courseId, new AddExamFormModel()
            {
                Name = "Test",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                IsPublic = true,
                Password = "Test"
            });

            var actualExamsCount = context.Exams.Count();

            Assert.Equal(expectedExamsCount, actualExamsCount);
        }

        [Fact]
        public async Task DoesTheUserHavePermissionToExamShouldReturnTrueIfUserIsEnrolledAndHasNotTakenTheExam()
        {
            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            var examId = SeedExam(courseId);
            var result = examService.DoesTheUserHavePermissionToExam(testUserId, examId);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesTheUserHavePermissionToExamShouldReturnFalseIfUserIsEnrolledAndHasTakenTheExam()
        {
            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedUserExam(courseId, testUserId);
            var examId = context.Exams.First().Id;

            var result = examService.DoesTheUserHavePermissionToExam(testUserId, examId);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesTheUserHavePermissionToExamShouldReturnFalseIfUserNotEnrolled()
        {
            var courseId = await SeedCourse();
            await SeedUser();

            var examId = SeedExam(courseId);

            var result = examService.DoesTheUserHavePermissionToExam(testUserId, examId);

            Assert.False(result);
        }

        [Fact]
        public async Task EditingTheExamShouldChangeItsName()
        {
            var expectedName = "Changed-Name";

            var courseId = await SeedCourse();
            var examId = SeedExam(courseId);

            var returnedExamId = await examService.EditExam(examId, new AddExamFormModel()
            {
                Name = expectedName,
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(15),
                IsPublic = true,
                Password = "HackMe"
            });

            var actualName = examService.GetExamById<BasicExamInfoViewModel>(examId).Name;

            Assert.Equal(expectedName, actualName);
            Assert.Equal(examId, returnedExamId);
        }

        [Fact]
        public async Task EvaluatingAnEmptyExamReturnsZeroPoints()
        {
            var expectedPoints = 0;

            var actualPoints = await examService.EvaluateExam(null, null);

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task GettingExamByIdShouldReturnTrueIfTheExamExists()
        {
            var courseId = await SeedCourse();
            var examId = SeedExam(courseId);

            var exists = examService.GetExamById(examId);

            Assert.True(exists);
        }

        [Fact]
        public void GettingExamByIdShouldReturnFalseIfTheExamDoesNotExist() =>
            Assert.False(examService.GetExamById("invalid-id"));

        [Fact]
        public async Task GettingExamsByCourseIdShouldReturnTheCorrectExams()
        {
            var courseId = await SeedCourse();
            var expectedExamId = SeedExam(courseId);

            var actualExamId = examService.GetExamsByCourseId<BasicExamInfoViewModel>(courseId).First().Id;

            Assert.Equal(expectedExamId, actualExamId);
        }

        [Fact]
        public async Task IsProvidedPasswordRightShouldReturnTrueIfPasswordIsCorrect()
        {
            var courseId = await SeedCourse();
            var examId = SeedExam(courseId);

            var password = "HackMe";
            var isPasswordCorrect = examService.IsProvidedPasswordRight(examId, password);

            Assert.True(isPasswordCorrect);
        }

        [Fact]
        public async Task IsProvidedPasswordRightShouldReturnFalseIfPasswordIsWrong()
        {
            var courseId = await SeedCourse();
            var examId = SeedExam(courseId);

            var password = "InvalidPassword";
            var isPasswordCorrect = examService.IsProvidedPasswordRight(examId, password);

            Assert.False(isPasswordCorrect);
        }

        [Fact]
        public async Task SavingUserExamShouldReturnInvalidCodeIfExamDateIsInvalid()
        {
            var expectedPoints = -1;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExpiredExam(courseId);
            var examId = context.Exams.First().Id;

            var actualPoints = await examService.SaveUserExam(testUserId, 999, examId);

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task SavingUserExamShouldReturnTheCorrectPoints()
        {
            var expectedPoints = 99;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;
            SeedExamQuestions(examId);

            var actualPoints = await examService.SaveUserExam(testUserId, 999, examId);

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task EvaluatingShouldReturnCorrectAmountOfPoints()
        {
            var expectedPoints = 10;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            var questions = GetTakeExamQuestionFormModels(examId);

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnQuestionWithoutAnswers()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = null,
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);

        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnQuestionWithMoreThanOneAnswers()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id",
                IsCorrect = true,
                Text = "test",
                QuestionId = "test-question-id",
            });
            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id-2",
                IsCorrect = false,
                Text = "You are dumb",
                QuestionId = "test-question-id",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test",Id = "test-answer-id"},
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test2",Id = "test-answer-id-2"}
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);

        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnQuestionWithNoChosenAnswers()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id",
                IsCorrect = true,
                Text = "test",
                QuestionId = "test-question-id",
            });
            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id-2",
                IsCorrect = false,
                Text = "You are dumb",
                QuestionId = "test-question-id",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = false,Text = "Test2",Id = "test-answer-id-2"}
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);

        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnInvalidQuestionId()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
            });

            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id",
                IsCorrect = true,
                Text = "test",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test2",Id = "test-answer-id-2"}
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);

        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnInvalidAnswerId()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.Answers.Add(new Answer()
            {
                IsCorrect = true,
                Text = "test",
                QuestionId = "test-question-id",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test",Id = "test-answer-id"},
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);

        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroPointsOnInvalidAnswer()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.Answers.Add(new Answer()
            {
                Id = "test-answer-id",
                IsCorrect = false,
                Text = "test",
                QuestionId = "test-question-id",
            });

            context.SaveChanges();

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test",Id = "test-answer-id"},
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);
        }

        [Fact]
        public async Task EvaluatingShouldReturnZeroWhenTheCorrectAnswerIdIsNotPresent()
        {
            var expectedPoints = 0;

            var courseId = await SeedCourse();
            await SeedUser();

            SeedUserCourse(courseId, testUserId);

            SeedExam(courseId);
            var examId = context.Exams.First().Id;

            SeedExamQuestionsAnswers(examId);

            context.Questions.Add(new Question()
            {
                ExamId = examId,
                Points = 10,
                Text = "Test-question",
                Id = "test-question-id"
            });

            context.SaveChanges();
            
            context.Answers.Add(new Answer()
            {
                IsCorrect = true,
                Text = "test",
                QuestionId = "test-question-id",
            });

            var questions = new List<TakeExamQuestionFormModel>()
            {
                new TakeExamQuestionFormModel()
                {
                    Id = "test-question-id",
                    Points = 10,
                    Text = "test-question",
                    Answers = new List<TakeExamQuestionAnswerFormModel>()
                    {
                        new TakeExamQuestionAnswerFormModel(){IsChosen = true,Text = "Test",Id = "test-answer-id-missing"},
                    },
                }
            };

            var actualPoints = await examService.EvaluateExam(new TakeExamFormModel()
            {
                Name = "Test-Exam",
                EndDate = DateTime.Today.AddDays(5),
                StartDate = DateTime.Today,
                IsPublic = true,
                Password = "HackMe",
                Questions = questions,
            }, testUserId);

            Assert.Equal(expectedPoints, actualPoints);
        }
    }
}
