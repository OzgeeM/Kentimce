using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;


namespace Kentimce.Application.Commands.ModerayonIslemleri
{
    public class SikayetModerasyon : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
        public bool OnaylandiMi { get; set; } // True: Yayına Al, False: Reddet
        public string RedNedeni { get; set; }
    }

    public class SikayetModerasyonHandler : IRequestHandler<SikayetModerasyon, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;

        public SikayetModerasyonHandler(IAppDbContext context) { _context = context; }

        public async Task<BaseResponse<string>> Handle(SikayetModerasyon request, CancellationToken token)
        {
            var sikayet = await _context.Sikayetler.FindAsync(request.SikayetId);
            if (sikayet == null) return new BaseResponse<string>().Fail("Şikayet bulunamadı");

            string bildirimMesaji = "";

            if (request.OnaylandiMi)
            {
                sikayet.Durum = SikayetDurumEnum.Yayinda.ToString(); // Artık herkes görebilir
                bildirimMesaji = $"'{sikayet.Baslik}' başlıklı şikayetiniz onaylandı ve yayına alındı.";
            }
            else
            {
                sikayet.Durum = SikayetDurumEnum.Reddedildi.ToString();
                bildirimMesaji = $"'{sikayet.Baslik}' başlıklı şikayetiniz reddedildi. Neden: {request.RedNedeni}";
            }

            // BİLDİRİM OLUŞTUR
            var bildirim = new Bildirim
            {
                Id = Guid.NewGuid(),
                KullaniciId = sikayet.KullaniciId.Value, // Şikayet sahibine
                Mesaj = bildirimMesaji,
                Link = $"/sikayet-detay/{sikayet.Id}",
                Tarih = DateTime.UtcNow
            };
            _context.Bildirimler.Add(bildirim);

            await _context.SaveChangesAsync(token);
            return new BaseResponse<string>().Success(request.OnaylandiMi ? "Onaylandı" : "Reddedildi");
        }
    }
}