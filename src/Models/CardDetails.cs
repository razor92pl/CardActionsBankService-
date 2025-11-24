using System;

namespace CardActionsBankService.Models
{
    /// <summary>
    /// Card details
    /// </summary>
    public record CardDetails(
        string CardNumber,
        CardType CardType,
        CardStatus CardStatus,
        bool IsPinSet
    );
}
