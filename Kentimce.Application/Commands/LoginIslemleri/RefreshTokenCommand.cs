using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Kentimce.Application.Commands.LoginIslemleri
{
    public class RefreshTokenCommand : IRequest<BaseResponse<AuthenticatedResponse>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, BaseResponse<AuthenticatedResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IJWTTokenService _tokenService;

        public RefreshTokenCommandHandler(IAppDbContext context, IJWTTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<AuthenticatedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<AuthenticatedResponse>();

            // 1. Eski Access Token'dan Kullanıcı Bilgisini (Principal) Çözümle
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return response.Fail("Geçersiz access token.");
            }

            // 2. Token içindeki User ID'yi al
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return response.Fail("Token geçersiz.");
            }

            // 3. Kullanıcıyı DB'den bul
            var user = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.Id == Guid.Parse(userIdClaim), cancellationToken);

            // 4. Kritik Kontroller
            if (user == null ||
                user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return response.Fail("Geçersiz veya süresi dolmuş refresh token. Lütfen tekrar giriş yapın.");
            }

            // 5. Yeni Access Token Üret
            // Rolleri tekrar çekiyoruz çünkü yetkileri değişmiş olabilir
            var userRoles = await _context.KullaniciRolleri
                                          .Where(kr => kr.KullaniciId == user.Id)
                                          .Select(kr => kr.rol.Isim)
                                          .ToListAsync(cancellationToken);

            var newAccessToken = _tokenService.GenerateAccessToken(user, userRoles);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // 6. Yeni Refresh Token'ı Kaydet
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _context.SaveChangesAsync(cancellationToken);

            // 7. Cevap Dön
            return response.Success(new AuthenticatedResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
