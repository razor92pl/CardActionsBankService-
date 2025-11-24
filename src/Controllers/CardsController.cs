using Microsoft.AspNetCore.Mvc;
using CardActionsBankService.Services;
using CardActionsBankService.DTOs;

namespace CardActionsBankService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        /// <summary>
        /// Returns allowed actions for a given card.
        /// </summary>
        [HttpGet("{userId}/{cardNumber}/actions")]
        [ProducesResponseType(typeof(AllowedActionsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AllowedActionsResponseDto>> GetAllowedActions(string userId, string cardNumber)
        {
            // --- parameterds validation ---
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(cardNumber))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error",
                    Detail = "UserId and CardNumber are required."
                });
            }

            if (userId.Length > 12 || cardNumber.Length > 16)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error",
                    Detail = "UserId must be <= 12 chars and CardNumber <= 16 chars."
                });
            }

            try
            {
                // --- get alloweed actions---
                var result = await _cardService.GetAllowedActions(userId, cardNumber);

                if (result is null)
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Card not found",
                        Detail = $"Card '{cardNumber}' for user '{userId}' not found."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal server error",
                    Detail = ex.Message
                });
            }
        }
    }
}
