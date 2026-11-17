using Kentimce.Application.Abstractions;
using Kentimce.Application.DTOs;
using Kentimce.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class GetVatandasSikayetleriQuery : IRequest<BaseResponse<List<SikayetListDto>>>
    {
    }
    public class GetVatandasSikayetleriQueryHandler : IRequestHandler<GetVatandasSikayetleriQuery, BaseResponse<List<SikayetListDto>>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetVatandasSikayetleriQueryHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<List<SikayetListDto>>> Handle(GetVatandasSikayetleriQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<SikayetListDto>>();

            // 1. Kullanıcı ID'sini Token'dan al
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi alınamadı.");
            }

           
            var sikayetler = await _context.Sikayetler
                .Include(s => s.belediye) 
                .Include(s => s.kullanici) 
                .Where(s => s.KullaniciId == currentUserId)
                .OrderByDescending(s => s.OlusturmaTarihi) 
                .Select(s => new SikayetListDto
                {
                    Id = s.Id,
                    Baslik = s.Baslik,
                    // Özet: İlk 80 karakter, yoksa tamamı
                    Ozet = s.Aciklama.Length > 80 ? s.Aciklama.Substring(0, 80) + "..." : s.Aciklama,
                    Adres = s.Adres,
                    Durum = s.Durum,
                    Tarih = s.OlusturmaTarihi,
                    KullaniciAdSoyad = $"{s.kullanici.Ad} {s.kullanici.Soyad}",
                    BelediyeAdi = s.belediye != null ? s.belediye.Isim : "Bilinmiyor"
                })
                .ToListAsync(cancellationToken);

            // Liste boşsa bile başarılı dön 
            if (sikayetler.Count == 0)
            {
                return response.Success(new List<SikayetListDto>());
            }

            return response.Success(sikayetler);
        }
    }
}