﻿namespace DeemZ.Services.LectureServices
{
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using DeemZ.Models.FormModels.Lecture;
    using DeemZ.Data.Models;
    using DeemZ.Models.FormModels.Description;
    using DeemZ.Services.ResourceService;
    using System;
    using System.Threading.Tasks;

    public class LectureService : ILectureService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;
        private readonly IResourceService resourceService;

        public LectureService(DeemZDbContext context, IMapper mapper, IResourceService resourceService)
        {
            this.context = context;
            this.mapper = mapper;
            this.resourceService = resourceService;
        }

        public async Task<string> AddLectureToCourse(string courseId, AddLectureFormModel lecture)
        {
            var newlyLecture = mapper.Map<Lecture>(lecture);
            newlyLecture.CourseId = courseId;

            if(lecture.Date != null) newlyLecture.Date = ((DateTime)lecture.Date).ToUniversalTime();

            context.Lectures.Add(newlyLecture);
            await context.SaveChangesAsync();

            return newlyLecture.Id;
        }

        public IEnumerable<T> GetLectureResourcesById<T>(string lid)
            => context.Resources
                .Where(x => x.LectureId == lid)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public async Task EditLectureById(string lectureId, EditLectureFormModel lecture)
        {
            var lectureToEdit = GetLectureById<Lecture>(lectureId);

            lectureToEdit.Name = lecture.Name;

            if (lecture.Date != null) lectureToEdit.Date = ((DateTime)lecture.Date).ToUniversalTime();

            for (int i = 0; i < lecture.Descriptions.Count; i++)
            {
                var (id, name) = GetDescriptionIdAndName(lecture.Descriptions[i]);
                var description = GetDescriptionById(id);
                if (description == null)
                {
                    if (name.Trim().Length < 3) continue;

                    description = CreateDescription(name , lectureId);
                    context.Descriptions.Add(description);
                }
                description.Name = name;
                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }

        private Description CreateDescription(string name, string lectureId)
        {
            var descripiton = new Description()
            {
                Name = name,
                LectureId = lectureId
            };

            return descripiton;
        }

        private (string id,string name) GetDescriptionIdAndName(EditDescriptionFormModel model)
        {
            var id = model.Id;
            var name = model.Name;
            return (id, name);
        }

        private Description GetDescriptionById(string did)
            => context.Descriptions.Find(did);

        public T GetLectureById<T>(string lid)
        {
            var lecture = context.Lectures
                .FirstOrDefault(x => x.Id == lid);

            return mapper.Map<T>(lecture);
        }

        public bool GetLectureById(string lid)
            => context.Lectures.Any(x => x.Id == lid);

        public List<T> GetLectureDescriptions<T>(string lid)
            => context.Descriptions
                .Where(x => x.LectureId == lid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public IEnumerable<T> GetLecturesByCourseId<T>(string cid)
            => context.Lectures
                .OrderByDescending(x => x.Date)
                .Include(x => x.Course)
                .Where(x => x.CourseId == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList();

        public async Task DeleteDescription(string did)
        {
            var description = GetDescriptionById(did);

            if (description == null) return;

            context.Descriptions.Remove(description);
            await context.SaveChangesAsync();
        }

        public async Task DeleteLecture(string lid)
        {
            var lecture = GetLectureById<Lecture>(lid);

            await resourceService.DeleteLectureResoureces(lid);
            await DeleteAllDescription(lid);

            context.Lectures.Remove(lecture);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAllDescription(string lid)
        {
            var descriptions = GetLectureDescriptions<Description>(lid);

            foreach (var description in descriptions)
            {
                await DeleteDescription(description.Id);
            }
        }
    }
}
