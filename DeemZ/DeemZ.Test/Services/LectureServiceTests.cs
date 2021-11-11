using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeemZ.Data.Models;
using DeemZ.Models.FormModels.Description;
using DeemZ.Models.FormModels.Lecture;
using DeemZ.Models.ViewModels.Description;
using DeemZ.Models.ViewModels.Lectures;
using DeemZ.Models.ViewModels.Resources;
using Xunit;

namespace DeemZ.Test.Services
{
    public class LectureServiceTests : BaseTestClass
    {
        public LectureServiceTests() => SetUpServices();

        [Fact]
        public async Task EditLectureByIdShouldEditTheLecture()
        {
            //Arrange
            var expectedName = "TEST";

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            //Act
            await lectureService.EditLectureById(lectureId, new EditLectureFormModel()
            {
                CourseId = courseId,
                Descriptions = new List<EditDescriptionFormModel>()
                    { new EditDescriptionFormModel() { Name = "test" } }
                ,
                Name = "TEST"
            });
            var lectureNameFromService = lectureService.GetLectureById<DetailsLectureViewModel>(lectureId).Name;

            //Assert
            Assert.Equal(expectedName, lectureNameFromService);
        }

        [Fact]
        public async Task EditingALectureWithDateShouldSetTheDateToUTCStandard()
        {
            var expectedDate = DateTime.Today.AddDays(-50);

            var courseId = await SeedCourse();

            var lectureId = "Test-Lecture-Id";

            context.Lectures.Add(new Lecture()
            {
                CourseId = courseId,
                Name = "Test Lecture",
                Id = lectureId,
            });
            await context.SaveChangesAsync();
            ;
            //Act
            await lectureService.EditLectureById(lectureId, new EditLectureFormModel()
            {
                CourseId = courseId,
                Descriptions = new List<EditDescriptionFormModel>()
                    { new EditDescriptionFormModel() { Name = "test" } }
                ,
                Date = expectedDate,
            });
            var lectureDateFromService = lectureService.GetLectureById<DetailsLectureViewModel>(lectureId).Date;

            //Assert
            Assert.Equal(expectedDate.ToUniversalTime(),lectureDateFromService);
        }

        [Fact]
        public async Task EditLectureByIdShouldNotEditTheLectureWhenTheDescriptionNameIsTooShort()
        {
            var expectedLecturesCount = 0;
            //Arrange
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            //Act
            await lectureService.EditLectureById(lectureId, new EditLectureFormModel()
            {
                CourseId = courseId,
                Name = "a",
                Descriptions = new List<EditDescriptionFormModel>()
                {
                    new EditDescriptionFormModel(){Name = "a"}
                }
            });
            var actualDescriptionsCount = lectureService.GetLectureById<DetailsLectureViewModel>(lectureId).Descriptions.Count;

            //Assert
            Assert.Equal(expectedLecturesCount, actualDescriptionsCount);
        }


        [Fact]
        public async Task GetLectureResourcesByIdShouldReturnTheCorrectResources()
        {
            var expectedResourceName = "Test";
            await SeedResource();
            var lectureId = context.Lectures.ToArray().First().Id;

            var resourceName = lectureService.GetLectureResourcesById<IndexResourceViewModel>(lectureId).First().Name;

            Assert.Equal(expectedResourceName, resourceName);

        }

        [Fact]
        public async Task GetLectureByIdShouldReturnTrueIfLectureIsPresent()
        {
            var courseId = await SeedCourse();
            var lectureId = await SeedLecture(courseId);

            var lectureExists = lectureService.GetLectureById(lectureId);

            Assert.True(lectureExists);
        }

        [Fact]
        public void GetLectureByIdShouldReturnFalseIfLectureIsNotPresent() => Assert.False(lectureService.GetLectureById("invalid-id"));

        [Fact]
        public async Task GettingLectureDescriptionsShouldReturnTheCorrectDescriptions()
        {
            var expectedDescriptionName = "test";

            var lectureId = await SeedLectureWithDescriptions();

            var actualDescriptionName = lectureService.GetLectureDescriptions<DetailsDescriptionViewModel>(lectureId).First().Name;

            Assert.Equal(expectedDescriptionName, actualDescriptionName);
        }

        [Fact]
        public async Task GettingLecturesByCourseIdShouldReturnTheCorrectLectures()
        {
            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            var lectureIdFromService = lectureService.GetLecturesByCourseId<DetailsLectureViewModel>(courseId).First().Id;

            Assert.Equal(lectureId, lectureIdFromService);
        }

        [Fact]
        public async Task DeletingLectureShouldRemoveItFromTheDb()
        {
            var expectedLecturesCount = 0;

            var courseId = await SeedCourse();

            var lectureId = await SeedLecture(courseId);

            await lectureService.DeleteLecture(lectureId);

            var actualLecturesCount = context.Lectures.ToArray().Length;

            Assert.Equal(expectedLecturesCount, actualLecturesCount);
        }

        [Fact]
        public async Task DeletingAllDescriptionsShouldRemoveTheLectureDescriptions()
        {
            var expectedLectureDescriptionsCount = 0;

            var lectureId = await SeedLectureWithDescriptions();

            await lectureService.DeleteAllDescription(lectureId);

            var actualLectureDescriptionsCount = lectureService.GetLectureDescriptions<DetailsDescriptionViewModel>(lectureId).Count;

            Assert.Equal(expectedLectureDescriptionsCount, actualLectureDescriptionsCount);
        }

        [Fact]
        public void DeletingNonExistantDescriptionShouldNotDoAnything()
        {
            var testDescriptionId = "test-description-id";

            lectureService.DeleteDescription(testDescriptionId);

            Assert.False(context.Descriptions.Any(x => x.Id == testDescriptionId));
        }
    }
}
