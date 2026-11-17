using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Commands.RegisterIslemleri
{
    public class Dogrulama : IRequest<BaseResponse<string>>
    {
        public string Email { get; set; }
        public string Kod { get; set; }
    }

    public class DogrulamaHandler : IRequestHandler<Dogrulama, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;

        public DogrulamaHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<string>> Handle(Dogrulama request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Kullanıcıyı Bul
            var kullanici = await _context.Kullanicilar
                                          .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (kullanici == null)
            {
                return response.Fail("Kullanıcı bulunamadı.");
            }

            // 2. Zaten doğrulanmış mı kontrolü
            if (kullanici.EmailDogrulandiMi)
            {
                return response.Fail("Bu hesap zaten doğrulanmış. Giriş yapabilirsiniz.");
            }

            // 3. Kod Kontrolü (Kod eşleşiyor mu?)
            if (kullanici.DogrulamaKodu != request.Kod)
            {
              
                return response.Fail("Girdiğiniz doğrulama kodu hatalı.");
            }

            // 4. Süre Kontrolü (5 dakika geçmiş mi?)
            if (kullanici.DogrulamaKoduGecerlilikSuresi < DateTime.UtcNow)
            {
                return response.Fail("Doğrulama kodunun süresi dolmuş. Lütfen tekrar kayıt olun veya yeni kod isteyin.");
            }

            // --- DOĞRULAMA BAŞARILI ---
            kullanici.EmailDogrulandiMi = true;
            // Güvenlik için kullanılan kodu temizle (Tekrar kullanılamasın)
            kullanici.DogrulamaKodu = null;
            await _context.SaveChangesAsync(cancellationToken);

            return response.Success("Hesabınız başarıyla doğrulandı! Artık giriş yapabilirsiniz.");
        }
    }
}


