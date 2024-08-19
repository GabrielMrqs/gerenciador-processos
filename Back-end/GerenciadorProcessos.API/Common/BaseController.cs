using GerenciadorProcessos.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProcessos.API.Common
{
    public abstract class BaseController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> HandleCommand<TSuccess, TFailure>(IRequest<Result<TSuccess, TFailure>> command)
        {
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Failure);
            }

            return Ok(result.Success);
        }
    }

}
