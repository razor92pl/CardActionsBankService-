using Xunit;
using CardActionsBankService.Models;
using CardActionsBankService.Rules;
using System.Linq;

namespace CardActionsBankService.Tests
{
    public class CardActionRulesTests
    {
        [Fact]
        public void ActiveCard_AllowsAction1()
        {
            var card = new CardDetails("123", CardType.Debit, CardStatus.Active, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION1", actions);
        }

        [Fact]
        public void InactiveCard_AllowsAction2()
        {
            var card = new CardDetails("456", CardType.Debit, CardStatus.Inactive, false);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION2", actions);
        }

        [Fact]
        public void CreditCard_AllowsAction5()
        {
            var card = new CardDetails("789", CardType.Credit, CardStatus.Active, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION5", actions);
        }

        [Fact]
        public void CardWithPin_AllowsAction6_NotAction7()
        {
            var card = new CardDetails("111", CardType.Debit, CardStatus.Active, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION6", actions);
            Assert.DoesNotContain("ACTION7", actions);
        }

        [Fact]
        public void CardWithoutPin_AllowsAction7_NotAction6()
        {
            var card = new CardDetails("222", CardType.Debit, CardStatus.Active, false);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION7", actions);
            Assert.DoesNotContain("ACTION6", actions);
        }

        [Fact]
        public void BlockedCard_StillAllowsGenericActions()
        {
            var card = new CardDetails("333", CardType.Debit, CardStatus.Blocked, false);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION3", actions); // always allowed
            Assert.Contains("ACTION4", actions); // always allowed
            Assert.Contains("ACTION8", actions); // allowed for blocked
        }

        [Fact]
        public void OrderedCard_AllowsAction8_10_12_13()
        {
            var card = new CardDetails("444", CardType.Debit, CardStatus.Ordered, false);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION8", actions);
            Assert.Contains("ACTION10", actions);
            Assert.Contains("ACTION12", actions);
            Assert.Contains("ACTION13", actions);
        }

        [Fact]
        public void InactiveCard_AllowsAction11()
        {
            var card = new CardDetails("555", CardType.Debit, CardStatus.Inactive, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION11", actions);
        }

        [Fact]
        public void AnyCard_AllowsAction9()
        {
            var card = new CardDetails("666", CardType.Debit, CardStatus.Active, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION9", actions);
        }                                 

        [Fact]
        public void ActiveCard_AllowsAction10_12_13()
        {
            var card = new CardDetails("777", CardType.Debit, CardStatus.Active, false);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION10", actions);
            Assert.Contains("ACTION12", actions);
            Assert.Contains("ACTION13", actions);
        }

        [Fact]
        public void ActiveCard_AllowsAction11()
        {
            var card = new CardDetails("888", CardType.Debit, CardStatus.Active, true);

            var actions = CardActionRules.GetAllowedActions(card).ToList();

            Assert.Contains("ACTION11", actions);
        }

        [Fact]
        public void NullCard_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CardActionRules.GetAllowedActions(null));
        }
    }
}
