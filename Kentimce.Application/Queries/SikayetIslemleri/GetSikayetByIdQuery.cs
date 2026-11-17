using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{

    public class GetSikayetByIdQuery : IRequest<BaseResponse<SikayetDetailDto>>
    {
        public Guid Id { get; set; }

        public GetSikayetByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetSikayetByIdQueryHandler : IRequestHandler<GetSikayetByIdQuery, BaseResponse<SikayetDetailDto>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetSikayetByIdQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<SikayetDetailDto>> Handle(GetSikayetByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<SikayetDetailDto>();

            // 1. Şikayeti tüm detaylarıyla çek
            var sikayet = await _context.Sikayetler
                .Include(s => s.kullanici) // Şikayet eden
                .Include(s => s.belediye)  // Hangi belediye

                // Yorumları ve yorumu yazan kişiyi, hatta o kişinin rolleri
                .Include(s => s.yorumlar)
                    .ThenInclude(sy => sy.kullanici)
                        .ThenInclude(k => k.roller)
                            .ThenInclude(kr => kr.rol)

                .Include(s => s.begeniler)

                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (sikayet == null)
            {
                return response.Fail("Şikayet bulunamadı.");
            }

            // 2. Şu anki kullanıcıyı al (Ben beğendim mi kontrolü için)
            Guid currentUserId = Guid.Empty;
            if (!string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()))
            {
                Guid.TryParse(_currentUserService.GetUserId().ToString(), out currentUserId);
            }

            // 3. DTO Mapleme (Mapping)
            var detailDto = new SikayetDetailDto
            {
                Id = sikayet.Id,
                Baslik = sikayet.Baslik,
                Aciklama = sikayet.Aciklama,
                Adres = sikayet.Adres,
                Durum = sikayet.Durum,
                Tarih = sikayet.OlusturmaTarihi,

                KullaniciId = sikayet.KullaniciId,
                KullaniciAdSoyad = $"{sikayet.kullanici.Ad} {sikayet.kullanici.Soyad}",

                BelediyeAdi = sikayet.belediye != null ? sikayet.belediye.Isim : "Belirtilmemiş",

                // Beğeni Hesaplamaları
                BegeniSayisi = sikayet.begeniler.Count, // Listeden say
                BenBegendimMi = sikayet.begeniler.Any(b => b.KullaniciId == currentUserId), // Ben var mıyım?

                // Yorumları Mapleme
                Yorumlar = sikayet.yorumlar
                    .OrderBy(y => y.eklemeTarihi) // Eskiden yeniye
                    .Select(y => new SikayetYorumDto
                    {
                        Id = y.Id,
                        Yorum = y.Yorum,
                        Tarih = y.eklemeTarihi,
                        YazanAdSoyad = $"{y.kullanici.Ad} {y.kullanici.Soyad}",
                        // Yorumu yapan kişinin "Personel" rolü var mı?
                        YetkiliYorumuMu = y.kullanici.roller.Any(r => r.rol.Isim == "Personel")
                    }).ToList()
            };

            return response.Success(detailDto);
        }
    }
}