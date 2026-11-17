using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Commands.LoginIslemleri
{
    public class KodTekrarGonder : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
    }

    public class KodTekrarGonderHandler : IRequestHandler<KodTekrarGonder, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public KodTekrarGonderHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<BaseResponse<string>> Handle(KodTekrarGonder request, CancellationToken cancellationToken)
        {
            var kullanici = await _context.Kullanicilar.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (kullanici == null) return new BaseResponse<string>().Fail("Kullanıcı bulunamadı.");
            if (kullanici.EmailDogrulandiMi) return new BaseResponse<string>().Fail("Bu hesap zaten doğrulanmış.");

            // Yeni kod üret
            var yeniKod = new Random().Next(100000, 999999).ToString();
            kullanici.DogrulamaKodu = yeniKod;
            kullanici.DogrulamaKoduGecerlilikSuresi = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync(cancellationToken);

            // Mail at
            string emailBody = $"Merhaba {kullanici.Ad},<br>Yeni kayıt kodunuz: <b>{yeniKod}</b>";
            await _emailService.SendEmailAsync(kullanici.Email, "Yeni Doğrulama Kodu", emailBody);

            return new BaseResponse<string>().Success("Yeni kod gönderildi.");
        }
    }
}