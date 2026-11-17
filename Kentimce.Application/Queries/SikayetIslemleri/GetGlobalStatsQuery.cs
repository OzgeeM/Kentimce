using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetGlobalStatsQuery : IRequest<BaseResponse<GlobalStatsDto>>
    {
    }

    public class GetGlobalStatsQueryHandler : IRequestHandler<GetGlobalStatsQuery, BaseResponse<GlobalStatsDto>>
    {
        private readonly IAppDbContext _context;

        public GetGlobalStatsQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<GlobalStatsDto>> Handle(GetGlobalStatsQuery request, CancellationToken cancellationToken)
        {
            // 1. Toplam (Yayında veya Çözüldü olanlar - Taslak/Red/OnayBekliyor hariç)
            var toplam = await _context.Sikayetler.CountAsync(s =>
                s.Durum == SikayetDurumEnum.Yayinda.ToString() ||
                s.Durum == SikayetDurumEnum.Cozuldu.ToString(), cancellationToken);

            // 2. Çözülenler
            var cozulen = await _context.Sikayetler.CountAsync(s =>
                s.Durum == SikayetDurumEnum.Cozuldu.ToString(), cancellationToken);

            // 3. Bekleyenler (Yayında olup henüz çözülmemişler)
            var bekleyen = await _context.Sikayetler.CountAsync(s =>
                s.Durum == SikayetDurumEnum.Yayinda.ToString(), cancellationToken);

            var stats = new GlobalStatsDto
            {
                Toplam = toplam,
                Cozulen = cozulen,
                Bekleyen = bekleyen
            };

            return new BaseResponse<GlobalStatsDto>().Success(stats);
        }
    }
}