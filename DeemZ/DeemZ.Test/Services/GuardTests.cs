using Xunit;

namespace DeemZ.Test.Services
{
    public class GuardTests : BaseTestClass
    {
        public GuardTests()
        {
            SetUpServices();
        }

        [Fact]
        public void AgainstNullShouldReturnTrueIfValueIsNull() => Assert.True(guard.AgainstNull(null));

        [Fact]
        public void AgainstNullShouldReturnFalseIfValueIsNotNull() => Assert.False(guard.AgainstNull("test"));
    }
}
