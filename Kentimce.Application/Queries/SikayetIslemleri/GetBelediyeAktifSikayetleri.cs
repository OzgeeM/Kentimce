using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetBelediyeAktifSikayetleri : IRequest<BaseResponse<List<SikayetListDto>>>
    {
    }
    public class GetBelediyeAktifSikayetleriHandler : IRequestHandler<GetBelediyeAktifSikayetleri, BaseResponse<List<SikayetListDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetBelediyeAktifSikayetleriHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<List<SikayetListDto>>> Handle(GetBelediyeAktifSikayetleri request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<SikayetListDto>>();

            // 1. Kullanıcı ID kontrolü
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi geçersiz.");
            }

            // 2. Kullanıcıyı getir (BelediyeId'si ile birlikte lazım)
            var personel = await _context.Kullanicilar
                                         .Include(k => k.roller).ThenInclude(kr => kr.rol)
                                         .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

            if (personel == null) return response.Fail("Kullanıcı bulunamadı.");

            // 3. Rol Kontrolü (Personel mi?)
            bool isPersonel = personel.roller.Any(r => r.rol.Isim == "Personel");
            if (!isPersonel)
            {
                return response.Fail("Bu ekranı görüntüleme yetkiniz yok.");
            }

            // 4. BELEDİYE KONTROLÜ
            if (personel.BelediyeId == null)
            {
                // Eğer adam personel ama belediyesi atanmamışsa sistem hatasıdır veya admin atamayı unutmuştur.
                return response.Fail("Hesabınıza tanımlı bir belediye bulunamadı. Lütfen yöneticiyle iletişime geçin.");
            }

            // 5. SORGULAMA (Filtreleme)
            // Hedef: (Bu Belediyenin Şikayetleri) VE (Aktif Durumdakiler)
            var query = _context.Sikayetler
                .Include(s => s.kullanici) // Şikayet sahibinin adını göstermek için
                .Where(s => s.BelediyeId == personel.BelediyeId) 
                .Where(s =>   
                            s.Durum != SikayetDurumEnum.Cozuldu.ToString() &&    // Çözülenleri görmesin (Aktif liste)
                            s.Durum != SikayetDurumEnum.Reddedildi.ToString()  // Reddedilenleri görmesin
                           );  // Arşivlenenleri görmesin

            // Not: "OnayBekliyor" ve "Yayinda" olanlar gelecektir.
            // OnayBekliyor -> Moderatör onaylamalı.
            // Yayinda -> Saha ekibi çözmeli.

            var result = await query
                .OrderByDescending(s => s.OlusturmaTarihi)
                .Select(s => new SikayetListDto
                {
                    Id = s.Id,
                    Baslik = s.Baslik,
                    Ozet = s.Aciklama.Length > 80 ? s.Aciklama.Substring(0, 80) + "..." : s.Aciklama,
                    Adres = s.Adres,
                    Durum = s.Durum,
                    Tarih = s.OlusturmaTarihi,
                    KullaniciAdSoyad = $"{s.kullanici.Ad} {s.kullanici.Soyad}"
                })
                .ToListAsync(cancellationToken);

            // Eğer liste boşsa bile Success dönüyoruz (Boş liste döner), hata değil.
            if (result.Count == 0)
            {
                return response.Success(new List<SikayetListDto>()); 
            }

            return response.Success(result);
        }
    }
}