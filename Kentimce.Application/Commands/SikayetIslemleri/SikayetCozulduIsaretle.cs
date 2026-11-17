using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class SikayetCozulduIsaretle : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
        //public string? MemnuniyetNotu { get; set; } 
        //public int Yildiz { get; set; } 
    }
    public class SikayetCozulduIsaretleCommandHandler : IRequestHandler<SikayetCozulduIsaretle, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SikayetCozulduIsaretleCommandHandler> _logger;

        public SikayetCozulduIsaretleCommandHandler(
            IAppDbContext context,
            ICurrentUserService currentUserService,
            ILogger<SikayetCozulduIsaretleCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(SikayetCozulduIsaretle request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Oturum Kontrolü
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi alınamadı.");
            }

            // 2. Şikayeti Getir
            var sikayet = await _context.Sikayetler.FirstOrDefaultAsync(s => s.Id == request.SikayetId, cancellationToken);

            if (sikayet == null)
            {
                return response.Fail("Şikayet bulunamadı.");
            }

            // 3. Yetki Kontrolü (Sadece Sahibi)
            if (sikayet.KullaniciId != currentUserId)
            {
                return response.Fail("Bu işlemi yapmaya yetkiniz yok. Sadece kendi şikayetlerinizi kapatabilirsiniz.");
            }

            // 4. Durum Kontrolü (Zaten çözülmüş mü?)
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


                // B. Tarihçe Ekle (SikayetDurum Tablosu)
                var durumLog = new SikayetDurum
                {
                    Id = Guid.NewGuid(),
                    SikayetId = sikayet.Id,
                    Durum = SikayetDurumEnum.Cozuldu, // Enum
                    IslemTarihi = DateTime.UtcNow,
                    IslemYapanId = currentUserId,
                    //Aciklama = string.IsNullOrEmpty(request.MemnuniyetNotu)
                    //    ? "Kullanıcı sorunun çözüldüğünü onayladı."
                    //    : $"Kullanıcı onayı: {request.MemnuniyetNotu} (Puan: {request.Yildiz})"
                    Aciklama = "Kullanıcı sorunun çözüldüğünü onayladı."
                };

                _context.SikayetDurumlari.Add(durumLog);

                await _context.SaveChangesAsync(cancellationToken);
                await _context.CommitTransactionAsync();

                return response.Success("Şikayetiniz çözüldü olarak işaretlendi. Geri bildiriminiz için teşekkürler!");
            }
            catch (Exception ex)
            {
                await _context.RollbackTransactionAsync();
                _logger.LogError($"Çözüldü işaretleme hatası: {ex.Message}");
                return response.Fail("İşlem sırasında bir hata oluştu.");
            }
        }
    }
}
