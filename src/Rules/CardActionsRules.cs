using System;
using System.Collections.Generic;
using System.Linq;
using CardActionsBankService.Models;

namespace CardActionsBankService.Rules
{
    /// <summary>
    /// Business rules for allowed card actions.
    /// Declarative, extensible, and sorted output using dictionary.
    /// </summary>
    public static class CardActionRules
    {
        // Action order (map: name -> index)
        private static readonly Dictionary<string,int> ActionOrder = new()
        {
            ["ACTION1"] = 1,
            ["ACTION2"] = 2,
            ["ACTION3"] = 3,
            ["ACTION4"] = 4,
            ["ACTION5"] = 5,
            ["ACTION6"] = 6,
            ["ACTION7"] = 7,
            ["ACTION8"] = 8,
            ["ACTION9"] = 9,
            ["ACTION10"] = 10,
            ["ACTION11"] = 11,
            ["ACTION12"] = 12,
            ["ACTION13"] = 13
        };

        private static readonly List<CardActionRule> Rules = new()
        {
            new("ACTION1", AllTypes(), OnlyStatuses(CardStatus.Active), c => true),
            new("ACTION2", AllTypes(), OnlyStatuses(CardStatus.Inactive), c => true),
            new("ACTION3", AllTypes(), AllStatuses(), c => true),
            new("ACTION4", AllTypes(), AllStatuses(), c => true),
            new("ACTION5", OnlyTypes(CardType.Credit), AllStatuses(), c => true),
            new("ACTION6", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked), c => c.IsPinSet),
            new("ACTION7", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked), c => !c.IsPinSet),
            new("ACTION8", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked), c => true),
            new("ACTION9", AllTypes(), AllStatuses(), c => true),
            new("ACTION10", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active), c => true),
            new("ACTION11", AllTypes(), OnlyStatuses(CardStatus.Inactive, CardStatus.Active), c => true),
            new("ACTION12", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active), c => true),
            new("ACTION13", AllTypes(), OnlyStatuses(CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active), c => true),
        };

        public static IEnumerable<string> GetAllowedActions(CardDetails? card)
        {
            if (card is null)
                throw new ArgumentNullException(nameof(card));

            var matched = Rules
                .Where(rule =>
                    rule.AllowedTypes.Contains(card.CardType) &&
                    rule.AllowedStatuses.Contains(card.CardStatus) &&
                    rule.Condition(card))
                .Select(rule => rule.Action)
                .Distinct()
                .OrderBy(action => ActionOrder[action]) // order 
                .ToList();

            return matched;
        }

        // Helpers
        private static HashSet<CardType> AllTypes() =>
            new(Enum.GetValues<CardType>());

        private static HashSet<CardType> OnlyTypes(params CardType[] types) =>
            new(types);

        private static HashSet<CardStatus> AllStatuses() =>
            new(Enum.GetValues<CardStatus>());

        private static HashSet<CardStatus> OnlyStatuses(params CardStatus[] statuses) =>
            new(statuses);
    }

    /// <summary>
    /// Rule definition for a single action.
    /// </summary>
    public record CardActionRule(
        string Action,
        HashSet<CardType> AllowedTypes,
        HashSet<CardStatus> AllowedStatuses,
        Func<CardDetails, bool> Condition
    );
}
