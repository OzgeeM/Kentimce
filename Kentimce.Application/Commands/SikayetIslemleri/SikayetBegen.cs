using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class SikayetBegen : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
    }
    public class SikayetBegenCommandHandler : IRequestHandler<SikayetBegen, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SikayetBegenCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<string>> Handle(SikayetBegen request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Kullanıcı Kontrolü
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi alınamadı. Lütfen tekrar giriş yapın.");
            }

           
            var sikayetVarMi = await _context.Sikayetler.AnyAsync(s =>
                s.Id == request.SikayetId &&
                (s.Durum == SikayetDurumEnum.Yayinda.ToString() || s.Durum == SikayetDurumEnum.Cozuldu.ToString()),
                cancellationToken);

            if (!sikayetVarMi)
            {
                return response.Fail("Beğenilmek istenen şikayet bulunamadı veya yayından kaldırılmış.");
            }

            // 3. Beğeni İşlemi
            var mevcutBegeni = await _context.SikayetBegenileri
                .FirstOrDefaultAsync(x => x.SikayetId == request.SikayetId && x.KullaniciId == currentUserId, cancellationToken);

            try
            {
                string mesaj = "";

                if (mevcutBegeni != null)
                {
                    _context.SikayetBegenileri.Remove(mevcutBegeni);
                    mesaj = "Beğeni geri alındı.";
                }
                else
                {
                    var yeniBegeni = new SikayetBegeni
                    {
                        Id = Guid.NewGuid(),
                        SikayetId = request.SikayetId,
                        KullaniciId = currentUserId,
                        Tarih = DateTime.UtcNow
                    };
                    _context.SikayetBegenileri.Add(yeniBegeni);
                    mesaj = "Şikayet beğenildi.";
                }

                await _context.SaveChangesAsync(cancellationToken);
                return response.Success(mesaj);
            }
            catch (Exception ex)
            {
                return response.Fail($"İşlem sırasında hata oluştu: {ex.Message}");
            }
        }
    }
}
