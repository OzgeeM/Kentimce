using Kentimce.Application.DTOs;
using Kentimce.Application.Queries.KategoriIslemleri;
using Kentimce.Application.Queries.ReferansIslemleri;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kentimce.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferansController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ReferansController(IMediator mediator)
        {

            _mediator = mediator;
        }


        [HttpGet("getBelediyeler")]
        [ProducesResponseType(typeof(BaseResponse<List<BelediyeDto>>), 200)]
        public async Task<IActionResult> getBelediyeler()
        {
            var response = await _mediator.Send(new TumBelediyeleriGetirQuery());
           
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("getTumKategoriler")]
        [ProducesResponseType(typeof(BaseResponse<List<KategoriDto>>), 200)]
        public async Task<IActionResult> getTumKategoriler()
        {
            var response = await _mediator.Send(new GetTumKategoriler());
           
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}