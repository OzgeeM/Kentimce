using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class BelediyeCozumBildir : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
        public string CozumAciklamasi { get; set; } 
    }

    public class BelediyeCozumBildirCommandHandler : IRequestHandler<BelediyeCozumBildir, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmailService _emailService; 
        private readonly ILogger<BelediyeCozumBildirCommandHandler> _logger;

        public BelediyeCozumBildirCommandHandler(
            IAppDbContext context,
            ICurrentUserService currentUserService,
            IEmailService emailService,
            ILogger<BelediyeCozumBildirCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(BelediyeCozumBildir request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Oturum Kontrolü
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi alınamadı.");
            }

            // 2. ROL KONTROLÜ: Sadece "Personel" rolü yapabilir
            
            var kullaniciRolleri = await _context.KullaniciRolleri
                                                 .Include(kr => kr.rol)
                                                 .Where(kr => kr.KullaniciId == currentUserId)
                                                 .Select(kr => kr.rol.Isim)
                                                 .ToListAsync(cancellationToken);

            if (!kullaniciRolleri.Contains("Personel"))
            {
                return response.Fail("Bu işlemi yapmaya yetkiniz yok. Sadece belediye personeli işlem yapabilir.");
            }

            // 3. Şikayeti Getir (Kullanıcı bilgisiyle beraber, mail atacağız çünkü)
            var sikayet = await _context.Sikayetler
                                        .Include(s => s.kullanici) 
                                        .FirstOrDefaultAsync(s => s.Id == request.SikayetId, cancellationToken);

            if (sikayet == null)
            {
                return response.Fail("Şikayet bulunamadı.");
            }

            // 4. Durum Kontrolü
            if (sikayet.Durum == SikayetDurumEnum.Cozuldu.ToString())
            {
                return response.Fail("Bu şikayet zaten çözüldü olarak işaretlenmiş.");
            }

            // --- TRANSACTION BAŞLAT ---
            await _context.BeginTransactionAsync();

            try
            {
                // A. Durumu Güncelle
                sikayet.Durum = SikayetDurumEnum.Cozuldu.ToString();

                // B. Tarihçe Ekle (Log)
                var durumLog = new SikayetDurum
                {
                    Id = Guid.NewGuid(),
                    SikayetId = sikayet.Id,
                    Durum = SikayetDurumEnum.Cozuldu,
                    IslemTarihi = DateTime.UtcNow,
                    IslemYapanId = currentUserId,
                    Aciklama = $"Belediye Çözümü: {request.CozumAciklamasi}"
                };

                _context.SikayetDurumlari.Add(durumLog);

                await _context.SaveChangesAsync(cancellationToken);

                // C. MAİL GÖNDERME (Kullanıcıya haber ver)
                if (sikayet.kullanici != null && !string.IsNullOrEmpty(sikayet.kullanici.Email))
                {
                    string konu = "Şikayetiniz Çözüme Kavuşturuldu";
                    string icerik = $@"
                        Merhaba Sayın {sikayet.kullanici.Ad} {sikayet.kullanici.Soyad},<br><br>
                        '{sikayet.Baslik}' başlıklı şikayetiniz belediye ekiplerimizce incelenmiş ve çözüme kavuşturulmuştur.<br><br>
                        <b>Belediye Açıklaması:</b><br>
                        <i>{request.CozumAciklamasi}</i><br><br>
                        Eğer sorunun devam ettiğini düşünüyorsanız lütfen uygulamamız üzerinden bildirimde bulununuz.<br>
                        Sağlıklı günler dileriz.";

                    // Mail hatası transaction'ı bozmasın diye iç try-catch
                    try
                    {
                        await _emailService.SendEmailAsync(sikayet.kullanici.Email, konu, icerik);
                    }
                    catch (Exception mailEx)
                    {
                        _logger.LogWarning($"Mail gönderilemedi: {mailEx.Message}");
                        // Mail gitmese de işlem veritabanında tamamlansın mı? Evet. 
                    }
                }

                // D. Commit
                await _context.CommitTransactionAsync();

                return response.Success("Çözüm bildirimi başarıyla kaydedildi ve vatandaşa bilgilendirme yapıldı.");
            }
            catch (Exception ex)
            {
                await _context.RollbackTransactionAsync();
                _logger.LogError($"Belediye çözüm bildirme hatası: {ex.Message}");
                return response.Fail("İşlem sırasında bir hata oluştu.");
            }
        }
    }

}
