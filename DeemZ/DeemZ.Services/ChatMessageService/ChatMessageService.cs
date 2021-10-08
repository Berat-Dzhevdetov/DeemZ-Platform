﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeemZ.Data;
using DeemZ.Data.Models;
using DeemZ.Models.FormModels.ChatMessage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeemZ.Services.ChatMessageService
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public ChatMessageService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public bool CanUserSendMessage(string courseId, string userId)
            => context.UserCourses
            .Any(x => x.UserId == userId && x.CourseId == courseId);

        public async Task DeleteChatMessage(string messsageId)
        {
            var message = context.Messages.FirstOrDefault(x => x.Id == messsageId);
            
            context.Messages.Remove(message);

            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllChatMessages<T>()
            => context.Messages
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToList();

        public ChatMessage GetChatMessageById(string messageId)
        => context.Messages
            .Where(x => x.Id == messageId)
            .Include(x => x.Course)
            .Include(x => x.ApplicationUser)
            .FirstOrDefault();

        public IEnumerable<T> GetChatMessagesByCourse<T>(string courseId)
        => context.Messages
            .Where(x=>x.CourseId == courseId)
            .ProjectTo<T>(mapper.ConfigurationProvider)
            .ToList();

        public async Task<string> SendChatMessage(ChatMessageInputModel inputModel)
        {
            var chatMessage = new ChatMessage()
            {
                ApplicationUserId = inputModel.ApplicationUserId,
                CourseId = inputModel.CourseId,
                Content = inputModel.Content,
            };

            context.Messages.Add(chatMessage);
            await context.SaveChangesAsync();

            return chatMessage.Id;

        }
    }
}