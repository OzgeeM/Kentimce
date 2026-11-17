using FluentValidation;
using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Commands.LoginIslemleri
{
    public class Login : IRequest<BaseResponse<AuthenticatedResponse>>
    {
        public string Email { get; set; }
        public string Sifre { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<Login, BaseResponse<AuthenticatedResponse>>
    {
        private readonly IAppDbContext _context;
        private readonly IJWTTokenService _tokenService;

        public LoginCommandHandler(IAppDbContext context, IJWTTokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<AuthenticatedResponse>> Handle(Login request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<AuthenticatedResponse>();

            // 1. Kullanıcıyı ve Rollerini Çek
            var user = await _context.Kullanicilar
                .Include(k => k.roller)
                    .ThenInclude(kr => kr.rol) 
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            // 2. Kullanıcı Var mı?
            if (user == null)
            {
                return response.Fail("Geçersiz e-posta veya şifre.");
            }

            // 3. Şifre Doğru mu?
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Sifre, user.ParolaHash);
            if (!isPasswordValid)
            {
                return response.Fail("Geçersiz e-posta veya şifre.");
            }

            // 4. E-Posta Doğrulanmış mı? 
            if (!user.EmailDogrulandiMi)
            {
                return response.Fail("Lütfen önce e-posta adresinizi doğrulayınız.");
            }

            // 5. Token Üret
            var roller = user.roller.Select(r => r.rol.Isim).ToList();
            var accessToken = _tokenService.GenerateAccessToken(user, roller);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // 6. Refresh Token'ı Veritabanına Kaydet
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); 

            await _context.SaveChangesAsync(cancellationToken);

            // 7. Cevap Dön
            return response.Success(new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }

    public class LoginCommandValidator : AbstractValidator<Login>
    {
        public LoginCommandValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir email giriniz.");
            RuleFor(p => p.Sifre).NotEmpty().WithMessage("Şifre boş olamaz.");
        }
    }
}
