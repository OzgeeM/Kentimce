using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;

namespace Kentimce.Application.Commands.KullaniciIslemleri
{
    public class SifreDegistirCommand : IRequest<BaseResponse<string>>
    {
        public string EskiSifre { get; set; }
        public string YeniSifre { get; set; }
    }

    public class SifreDegistirHandler : IRequestHandler<SifreDegistirCommand, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
   

        public SifreDegistirHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<string>> Handle(SifreDegistirCommand request, CancellationToken cancellationToken)
        {
            var userIdStr = _currentUserService.GetUserId();
            if (userIdStr == null) return new BaseResponse<string>().Fail("Oturum yok");
            Guid userId = Guid.Parse(userIdStr.ToString());

            var user = await _context.Kullanicilar.FindAsync(userId);
            if (user == null) return new BaseResponse<string>().Fail("Kullanıcı bulunamadı");

            bool eskiSifreDogruMu = BCrypt.Net.BCrypt.Verify(request.EskiSifre, user.ParolaHash);

            if (!eskiSifreDogruMu)
            {
                return new BaseResponse<string>().Fail("Eski şifreniz hatalı.");
            }

        
            string yeniHash = BCrypt.Net.BCrypt.HashPassword(request.YeniSifre);

            user.ParolaHash = yeniHash;

            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse<string>().Success("Şifreniz başarıyla güncellendi.");
        }
    }
}