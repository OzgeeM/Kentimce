using Kentimce.Application.Commands.LoginIslemleri;
using Kentimce.Application.Commands.RegisterIslemleri;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Kentimce.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
      
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
          
            _mediator = mediator;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(BaseResponse<string>), 401)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<BaseResponse<string>> Register(Kayit command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPost("MailDogrulama")]
        [ProducesResponseType(typeof(BaseResponse<string>), 401)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<BaseResponse<string>> MailDogrulama(Dogrulama command)
        {
            var response = await _mediator.Send(command);
            return response;

        }

        [HttpPost("KodTekrarGonder")]
        public async Task<IActionResult> KodTekrarGonder([FromBody] KodTekrarGonder request)
        {
            var response = await _mediator.Send(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        [HttpPost("Login")]
        [ProducesResponseType(typeof(BaseResponse<string>), 401)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticatedResponse>), 200)]
        public async Task<IActionResult> Login(Login command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

      
        [HttpPost("RefreshToken")]
        [ProducesResponseType(typeof(BaseResponse<string>), 401)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        [ProducesResponseType(typeof(BaseResponse<AuthenticatedResponse>), 200)]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


    }
}