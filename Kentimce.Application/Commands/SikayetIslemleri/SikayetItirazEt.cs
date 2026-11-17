using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class SikayetItirazEt : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
        public string ItirazAciklamasi { get; set; }
    }

    public class SikayetItirazEtHandler : IRequestHandler<SikayetItirazEt, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SikayetItirazEtHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<string>> Handle(SikayetItirazEt request, CancellationToken cancellationToken)
        {
            var userIdStr = _currentUserService.GetUserId();
            if (userIdStr == null) return new BaseResponse<string>().Fail("Oturum yok");
            Guid currentUserId = Guid.Parse(userIdStr.ToString());

            var sikayet = await _context.Sikayetler.FirstOrDefaultAsync(s => s.Id == request.SikayetId);
            if (sikayet == null) return new BaseResponse<string>().Fail("Kayıt bulunamadı");

            // Sadece sahibi itiraz edebilir
            if (sikayet.KullaniciId != currentUserId)
                return new BaseResponse<string>().Fail("Yetkiniz yok.");

            // Sadece Çözüldü ise itiraz edilebilir
            if (sikayet.Durum != SikayetDurumEnum.Cozuldu.ToString())
                return new BaseResponse<string>().Fail("Sadece çözülmüş şikayetlere itiraz edilebilir.");

            // DURUMU GERİ AL (YAYINDA)
            sikayet.Durum = SikayetDurumEnum.Yayinda.ToString();

            // LOG
            _context.SikayetDurumlari.Add(new SikayetDurum
            {
                Id = Guid.NewGuid(),
                SikayetId = sikayet.Id,
                Durum = SikayetDurumEnum.Yayinda,
                IslemTarihi = DateTime.UtcNow,
                IslemYapanId = currentUserId,
                Aciklama = $"Vatandaş İtirazı: {request.ItirazAciklamasi}"
            });

            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResponse<string>().Success("Şikayet tekrar işleme alındı.");
        }
    }
}