using Kentimce.Application.Queries.BildirimIslemleri;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kentimce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BildirimController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BildirimController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetBildirimlerim")]
        public async Task<IActionResult> GetBildirimlerim()
        {
            var response = await _mediator.Send(new GetKullaniciBildirimleri());
            return Ok(response);
        }
    }
}
