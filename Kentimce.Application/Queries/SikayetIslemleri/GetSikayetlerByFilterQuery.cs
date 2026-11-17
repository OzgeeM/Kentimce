using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetSikayetlerByFilterQuery : IRequest<BaseResponse<List<SikayetListDto>>>
    {
        public Guid? KategoriId { get; set; }
        public Guid? BelediyeId { get; set; }
        public string? Durum { get; set; } // "Cozuldu", "Yayinda" veya "all"
    }

    public class GetSikayetlerByFilterQueryHandler : IRequestHandler<GetSikayetlerByFilterQuery, BaseResponse<List<SikayetListDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetSikayetlerByFilterQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<List<SikayetListDto>>> Handle(GetSikayetlerByFilterQuery request, CancellationToken cancellationToken)
        {
            // Kullanıcı ID (Beğeni kontrolü için)
            Guid currentUserId = Guid.Empty;
            if (!string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()))
                Guid.TryParse(_currentUserService.GetUserId().ToString(), out currentUserId);

            // Temel Query
            var query = _context.Sikayetler.AsNoTracking().AsQueryable();

            // --- FILTRELER ---

            // 1. Kategori Filtresi
            if (request.KategoriId.HasValue && request.KategoriId != Guid.Empty)
            {
                query = query.Where(s => s.KategoriId == request.KategoriId);
            }

            // 2. Belediye Filtresi
            if (request.BelediyeId.HasValue && request.BelediyeId != Guid.Empty)
            {
                query = query.Where(s => s.BelediyeId == request.BelediyeId);
            }

            // 3. Durum Filtresi
            // Eğer durum "all" veya boş ise: Sadece Yayinda ve Cozuldu olanları getir (Taslak/Red gelmesin)
            if (string.IsNullOrEmpty(request.Durum) || request.Durum == "all")
            {
                query = query.Where(s => s.Durum == SikayetDurumEnum.Yayinda.ToString() ||
                                         s.Durum == SikayetDurumEnum.Cozuldu.ToString());
            }
            else
            {
                // Spesifik durum seçildiyse (Örn: Sadece Cozuldu)
                query = query.Where(s => s.Durum == request.Durum);
            }

            // --- SIRALAMA VE PROJECTION ---
            var result = await query
                .OrderByDescending(s => s.OlusturmaTarihi)
                .Select(s => new SikayetListDto
                {
                    Id = s.Id,
                    Baslik = s.Baslik,
                    Ozet = s.Aciklama.Length > 100 ? s.Aciklama.Substring(0, 100) + "..." : s.Aciklama,
                    Adres = s.Adres,
                    Durum = s.Durum,
                    Tarih = s.OlusturmaTarihi,
                    KullaniciAdSoyad = s.kullanici != null ? $"{s.kullanici.Ad} {s.kullanici.Soyad}" : "Gizli",
                    BelediyeAdi = s.belediye != null ? s.belediye.Isim : "Belirtilmemiş",
                    KategoriAdi = s.Kategori != null ? s.Kategori.Ad : "",
                    KategoriIkon = s.Kategori != null ? s.Kategori.Ikon : "",
                    KategoriRenk = s.Kategori != null ? s.Kategori.RenkKodu : "primary",
                    BegeniSayisi = s.begeniler.Count,
                    YorumSayisi = s.yorumlar.Count,
                    BenBegendimMi = s.begeniler.Any(b => b.KullaniciId == currentUserId)
                })
                .ToListAsync(cancellationToken);

            return new BaseResponse<List<SikayetListDto>>().Success(result);
        }
    }
}