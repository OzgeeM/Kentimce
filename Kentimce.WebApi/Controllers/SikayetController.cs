using Kentimce.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Kentimce.Application.Commands.SikayetIslemleri;
using Kentimce.Application.DTOs;
using Kentimce.Application.Queries.SikayetIslemleri;



namespace Kentimce.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SikayetController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SikayetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Ekle")]
        [ProducesResponseType(typeof(BaseResponse<string>), 401)]
        [ProducesResponseType(typeof(BaseResponse<string>), 404)]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<BaseResponse<string>> Ekle(SikayetEkle command)
        {
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpGet("{id}")] 
        [ProducesResponseType(typeof(BaseResponse<SikayetDetailDto>), 200)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetSikayetByIdQuery(id);
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response); 
        }


        [HttpGet("GetBelediyeSikayetleri")]
        [ProducesResponseType(typeof(BaseResponse<List<SikayetListDto>>), 200)]
        public async Task<IActionResult> GetBelediyeSikayetleri()
        {
            // Query nesnesi boş, çünkü parametre yok
            var query = new GetBelediyeAktifSikayetleri();
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("GetSikayetlerim")] 
        [ProducesResponseType(typeof(BaseResponse<List<SikayetListDto>>), 200)]
        public async Task<IActionResult> GetSikayetlerim()
        {
            var query = new GetVatandasSikayetleriQuery();
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpPost("YorumEkle")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> YorumEkle([FromBody] SikayetYorumEkle command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("Begen")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> Begen([FromBody] SikayetBegen command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess) 
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("CozulduIsaretleVatandas")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> CozulduIsaretleVatandas([FromBody] SikayetCozulduIsaretle command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpPost("CozumBildir")] 
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        public async Task<IActionResult> CozumBildir([FromBody] BelediyeCozumBildir command)
        {
            var response = await _mediator.Send(command);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpGet("GetGlobalFeedQuery")]
        [ProducesResponseType(typeof(BaseResponse<List<SikayetListDto>>), 200)]
        public async Task<IActionResult> GetGlobalFeedQuery([FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var query = new GetGlobalFeedQuery();
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
        [HttpGet("GetGlobalStatsQuery")]
        [ProducesResponseType(typeof(BaseResponse<GlobalStatsDto>), 200)]
        public async Task<IActionResult> GetGlobalStatsQuery()
        {
            var query = new GetGlobalStatsQuery();
            var response = await _mediator.Send(query);

            if (response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("GetByFilter")]
        public async Task<IActionResult> GetByFilter([FromBody] GetSikayetlerByFilterQuery query)
        {
            var response = await _mediator.Send(query);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("GetOnayBekleyenler")]
        [ProducesResponseType(typeof(BaseResponse<List<SikayetListDto>>), 200)]
        public async Task<IActionResult> GetOnayBekleyenler()
        {
            var query = new GetOnayBekleyenlerQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("ItirazEt")]
        public async Task<IActionResult> ItirazEt([FromBody] SikayetItirazEt command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }


    }

}
