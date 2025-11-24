using CardActionsBankService.DTOs;
using CardActionsBankService.Models;
using CardActionsBankService.Rules;

namespace CardActionsBankService.Services
{
    /// <summary>
    /// Domain service responsible for fetching card data
    /// and applying business rules to determine allowed actions.
    /// </summary>
    public class CardService : ICardService
    {
        // Provided sample card data (directly from the task)
        private readonly Dictionary<string, Dictionary<string, CardDetails>> _userCards = CreateSampleUserCards();

        // Returns allowed actions for a given card or null if card does not exist.
        public async Task<AllowedActionsResponseDto?> GetAllowedActions(string userId, string cardNumber)
        {
            var card = await GetCardDetails(userId, cardNumber);    
            if (card == null)
                return null;

            var actions = CardActionRules.GetAllowedActions(card);

            return new AllowedActionsResponseDto(actions);
        }

        // Example external-card-service simulation (from the task).
        public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
        {
            // At this point, we would typically make an HTTP call to an external service
            // to fetch the data. For this example we use generated sample data.
            await Task.Delay(1000);
            
            if (!_userCards.TryGetValue(userId, out var cards) || !cards.TryGetValue(cardNumber, out var cardDetails))
            {
                return null;
            }
            
            return cardDetails;
        }

        // ------------------------------
        // Sample data provided in task
        // ------------------------------
        private static Dictionary<string, Dictionary<string, CardDetails>> CreateSampleUserCards()
        {
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();

            for (var i = 1; i <= 3; i++)
            {
                var cards = new Dictionary<string, CardDetails>();
                var cardIndex = 1;

                foreach (CardType type in Enum.GetValues(typeof(CardType)))
                {
                    foreach (CardStatus status in Enum.GetValues(typeof(CardStatus)))
                    {
                        var cardNumber = $"Card{i}{cardIndex}";

                        cards.Add(cardNumber, new CardDetails(
                            CardNumber: cardNumber,
                            CardType: type,
                            CardStatus: status,
                            IsPinSet: cardIndex % 2 == 0
                        ));

                        cardIndex++;
                    }
                }

                var userId = $"User{i}";
                userCards.Add(userId, cards);
            }

            return userCards;
        }
    }
}



