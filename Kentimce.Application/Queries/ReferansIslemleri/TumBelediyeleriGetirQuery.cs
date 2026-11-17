using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.ReferansIslemleri
{
    
    public class TumBelediyeleriGetirQuery : IRequest<BaseResponse<List<BelediyeDto>>>
    {
       
    }

    public class TumBelediyeleriGetirHandler : IRequestHandler<TumBelediyeleriGetirQuery, BaseResponse<List<BelediyeDto>>>
    {
        private readonly IAppDbContext _context;

        public TumBelediyeleriGetirHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<BelediyeDto>>> Handle(TumBelediyeleriGetirQuery request, CancellationToken cancellationToken)
        {
           
            var belediyeler = await _context.Belediyeler
                .AsNoTracking()
                .OrderBy(x => x.Isim)
                .Select(x => new BelediyeDto
                {
                    Id = x.Id,
                    Isim = x.Isim,
                    Domain = x.Domain,
                    Tip = x.Tip
                })
                .ToListAsync(cancellationToken);

            return new BaseResponse<List<BelediyeDto>>().Success(belediyeler);
        }
    }
}