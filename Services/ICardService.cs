using System.Threading.Tasks;
using CardActionsService.DTOs;
using CardActionsService.Models;

namespace CardActionsService.Services
{
    public interface ICardService
    {
        Task<AllowedActionsResponseDto?> GetAllowedActions(string userId, string cardNumber);
        Task<CardDetails?> GetCardDetails(string userId, string cardNumber);
    }
}
