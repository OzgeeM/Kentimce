using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Queries.KategoriIslemleri
{
    public class GetTumKategoriler : IRequest<BaseResponse<List<KategoriDto>>>
    {
       
    }


    public class GetTumKategorilerHandler : IRequestHandler<GetTumKategoriler, BaseResponse<List<KategoriDto>>>
    {
        private readonly IAppDbContext _context;

        public GetTumKategorilerHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<KategoriDto>>> Handle(GetTumKategoriler request, CancellationToken cancellationToken)
        {
            var kategoriler = await _context.SikayetKategorileri
                                            .Where(x => x.AktifMi)
                                            .Select(x => new KategoriDto
                                            {
                                                Id = x.Id,
                                                Ad = x.Ad,
                                                Ikon = x.Ikon,
                                                RenkKodu = x.RenkKodu
                                            })
                                            .ToListAsync(cancellationToken);

            return new BaseResponse<List<KategoriDto>>().Success(kategoriler);
        }
    }
}
