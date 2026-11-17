using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetGlobalFeedQuery : IRequest<BaseResponse<List<SikayetListDto>>>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }

    public class GetGlobalFeedQueryHandler : IRequestHandler<GetGlobalFeedQuery, BaseResponse<List<SikayetListDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetGlobalFeedQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<List<SikayetListDto>>> Handle(GetGlobalFeedQuery request, CancellationToken cancellationToken)
        {
            // 1. Giriş yapan kullanıcının ID'sini al (Ben beğendim mi kontrolü için)
            Guid currentUserId = Guid.Empty;
            var userIdString = _currentUserService.GetUserId()?.ToString();
            if (!string.IsNullOrEmpty(userIdString))
            {
                Guid.TryParse(userIdString, out currentUserId);
            }

            // 2. Sorgu 
            var query = _context.Sikayetler
                .AsNoTracking()
                .Include(s => s.kullanici)
                .Include(s => s.belediye)
                .Include(s => s.Kategori)
                .Include(s => s.begeniler) // Sayı hesabı için
                .Include(s => s.yorumlar)  // Sayı hesabı için
                                           // Sadece Yayında veya Çözüldü olanları getir
                .Where(s => s.Durum == SikayetDurumEnum.Yayinda.ToString() ||
                            s.Durum == SikayetDurumEnum.Cozuldu.ToString())
                .OrderByDescending(s => s.OlusturmaTarihi);

            // 3. Projeksiyon (DTO'ya Çevirme)
            var list = await query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .Select(s => new SikayetListDto
                {
                    Id = s.Id,
                    Baslik = s.Baslik,
                    // Özet: Açıklama uzunsa kes
                    Ozet = s.Aciklama.Length > 100 ? s.Aciklama.Substring(0, 100) + "..." : s.Aciklama,
                    Adres = s.Adres,
                    Durum = s.Durum,
                    Tarih = s.OlusturmaTarihi,

                    // İlişkili tablolardan veriler
                    KullaniciAdSoyad = s.kullanici != null ? $"{s.kullanici.Ad} {s.kullanici.Soyad}" : "Gizli",
                    BelediyeAdi = s.belediye != null ? s.belediye.Isim : "Belirtilmemiş",
                    KategoriAdi = s.Kategori != null ? s.Kategori.Ad : "Genel",
                    KategoriRenk = s.Kategori != null ? s.Kategori.RenkKodu : "primary",
                    KategoriIkon = s.Kategori != null ? s.Kategori.Ikon : "ri-discuss-line",

                    BegeniSayisi = s.begeniler.Count(),
                    YorumSayisi = s.yorumlar.Count(),

                    // Kullanıcı bu şikayeti beğenmiş mi?
                    BenBegendimMi = s.begeniler.Any(b => b.KullaniciId == currentUserId)
                })
                .ToListAsync(cancellationToken);

            return new BaseResponse<List<SikayetListDto>>().Success(list);
        }
    }
}