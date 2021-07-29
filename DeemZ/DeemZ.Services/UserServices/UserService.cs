namespace DeemZ.Services.UserServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DeemZ.Data;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public UserService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<T> GetAllUsers<T>(int page = 1, int quantity = 20)
            => context.Users
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .ToList()
                .Paging(page, quantity);

        public int GetUserTakenCourses(string uid)
            => context.UserCourses
                .Count(x => x.IsPaid == true && x.UserId == uid);
    }
}
