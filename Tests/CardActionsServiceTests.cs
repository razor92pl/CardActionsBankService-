using Xunit;
using CardActionsBankService.Services;

namespace CardActionsBankService.Tests
{
    public class CardServiceTests
    {
        [Fact]
        public async Task GetAllowedActions_ReturnsExpectedActions_ForInactiveDebitWithPin()
        {
            var service = new CardService();

            var response = await service.GetAllowedActions("User1", "Card12");

            Assert.NotNull(response);
            Assert.Contains("ACTION2", response!.AllowedActions); // Inactive → ACTION2
            Assert.Contains("ACTION6", response.AllowedActions);  // PIN → ACTION6
        }                   

        [Fact]
        public async Task GetAllowedActions_ReturnsNull_WhenUserNotFound()
        {
            var service = new CardService();

            var response = await service.GetAllowedActions("UnknownUser", "Card12");

            Assert.Null(response);
        }       

        [Fact]
        public async Task GetAllowedActions_ReturnsNull_WhenCardNotFound()
        {
            var service = new CardService();

            var response = await service.GetAllowedActions("User1", "NonExistingCard");

            Assert.Null(response);
        }

    }
}
