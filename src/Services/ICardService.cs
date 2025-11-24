using System.Threading.Tasks;
using CardActionsBankService.DTOs;
using CardActionsBankService.Models;

namespace CardActionsBankService.Services
{
    public interface ICardService
    {
        Task<AllowedActionsResponseDto?> GetAllowedActions(string userId, string cardNumber);
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
    }
}
