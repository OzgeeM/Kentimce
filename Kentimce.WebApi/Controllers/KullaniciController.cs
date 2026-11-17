using Kentimce.Application.Abstractions;
using Kentimce.Application.Commands.KullaniciIslemleri;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context; 
        private readonly ICurrentUserService _currentUserService;

        public KullaniciController(IMediator mediator, IAppDbContext context, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _context = context;
            _currentUserService = currentUserService;
        }

        // 1. PROFİL BİLGİLERİNİ GETİR
        [HttpGet("GetProfil")]
        public async Task<IActionResult> GetProfil()
        {
            var userIdStr = _currentUserService.GetUserId();
            if (userIdStr == null) return Unauthorized();
            Guid userId = Guid.Parse(userIdStr.ToString());

           
            var user = await _context.Kullanicilar
                .Include(u => u.Belediye) 
                .Include(u => u.roller)   
                    .ThenInclude(ur => ur.rol) 
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound("Kullanıcı bulunamadı");

       
            var rollerString = user.roller != null
                ? string.Join(", ", user.roller.Select(r => r.rol.Isim))
                : "Kullanıcı";

            var dto = new
            {
                Ad = user.Ad,
                Soyad = user.Soyad,
                TcNo = user.Tc, 
                Email = user.Email,
                BelediyeAdi = user.Belediye != null ? user.Belediye.Isim : "Belirtilmemiş",
                Rol = rollerString
            };

            return Ok(new BaseResponse<object>().Success(dto));
        }

        // 2. ŞİFRE DEĞİŞTİRME
        [HttpPost("SifreDegistir")]
        public async Task<IActionResult> SifreDegistir([FromBody] SifreDegistirCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.IsSuccess) return Ok(response);
            return BadRequest(response);
        }
    }
}