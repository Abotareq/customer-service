using CustomerService.Application.Message.Commands.SendMessage;
using CustomerService.Application.Message.Queries.GetMessagesByRequestId;
using CustomerService.Contracts.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustomerService.Api.Controllers
{
    [Authorize]
    [Route("api/requests/{requestId:guid}/messages")]
    public sealed class MessagesController : ApiController
    {
        private readonly ISender _mediator;

        public MessagesController(ISender mediator)
        {
            _mediator = mediator;
        }

        private Guid CurrentUserId => Guid.Parse(User.FindFirstValue("sub")!);
        private string CurrentUserRole => User.FindFirstValue(ClaimTypes.Role)!;

        [HttpPost]
        public async Task<IActionResult> Send(Guid requestId, SendMessageRequest request)
        {
            var command = new SendMessageCommand(requestId, CurrentUserId, request.Content);
            var result = await _mediator.Send(command);
            return result.Match(response => Ok(response), Problem);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid requestId)
        {
            var query = new GetMessagesByRequestIdQuery(requestId, CurrentUserId, CurrentUserRole);
            var result = await _mediator.Send(query);
            return result.Match(response => Ok(response), Problem);
        }
    }
}
