using Kentimce.Application.Commands.ModerayonIslemleri;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kentimce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModerayonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModerayonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SikayetModerasyon")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> SikayetModerasyon([FromBody] SikayetModerasyon command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
