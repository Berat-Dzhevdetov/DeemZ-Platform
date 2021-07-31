namespace DeemZ.Test.Mocking
{
    using Moq;
    using DeemZ.Services.UserServices;

    public static class UserServiceMock
    {
        public static IUserService Instance
        {
            get
            {
                var serviceMock = new Mock<IUserService>();

                return serviceMock.Object;
            }
        }
    }
}