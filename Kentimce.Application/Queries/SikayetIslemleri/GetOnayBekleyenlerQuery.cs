using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetOnayBekleyenlerQuery : IRequest<BaseResponse<List<SikayetListDto>>> { }

    public class GetOnayBekleyenlerHandler : IRequestHandler<GetOnayBekleyenlerQuery, BaseResponse<List<SikayetListDto>>>
    {
        private readonly IAppDbContext _context;
        public GetOnayBekleyenlerHandler(IAppDbContext context) { _context = context; }

        public async Task<BaseResponse<List<SikayetListDto>>> Handle(GetOnayBekleyenlerQuery request, CancellationToken token)
        {
            var list = await _context.Sikayetler
                .Include(s => s.kullanici)
                .Where(s => s.Durum == SikayetDurumEnum.OnayBekliyor.ToString())
                .OrderBy(s => s.OlusturmaTarihi)
                .Select(s => new SikayetListDto
                {
                    Id = s.Id,
                    Baslik = s.Baslik,
                    Ozet = s.Aciklama.Substring(0, Math.Min(s.Aciklama.Length, 100)) + "...",
                    KullaniciAdSoyad = $"{s.kullanici.Ad} {s.kullanici.Soyad}",
                    Tarih = s.OlusturmaTarihi,
                    Durum = s.Durum
                }).ToListAsync(token);

            return new BaseResponse<List<SikayetListDto>>().Success(list);
        }
    }
}