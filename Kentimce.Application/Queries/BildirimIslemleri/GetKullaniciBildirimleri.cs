using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Queries.BildirimIslemleri
{
    public class GetKullaniciBildirimleri : IRequest<BaseResponse<List<BildirimDto>>>
    {
    }


    public class GetKullaniciBildirimleriHandler : IRequestHandler<GetKullaniciBildirimleri, BaseResponse<List<BildirimDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetKullaniciBildirimleriHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<List<BildirimDto>>> Handle(GetKullaniciBildirimleri request, CancellationToken cancellationToken)
        {
            var userIdStr = _currentUserService.GetUserId();
            if (userIdStr == null) return new BaseResponse<List<BildirimDto>>().Fail("Oturum yok");

            Guid userId = Guid.Parse(userIdStr.ToString());

            var bildirimler = await _context.Bildirimler
                .Where(b => b.KullaniciId == userId)
                .OrderByDescending(b => b.Tarih)
                .Take(50) // Son 50 bildirim
                .Select(b => new BildirimDto
                {
                    Id = b.Id,
                    Mesaj = b.Mesaj,
                    Link = b.Link,
                    Tarih = b.Tarih,
                    OkunduMu = false
                })
                .ToListAsync(cancellationToken);

            return new BaseResponse<List<BildirimDto>>().Success(bildirimler);
        }
    }
}