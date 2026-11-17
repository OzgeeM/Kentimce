using FluentValidation;
using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using Kentimce.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;


namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class SikayetEkle : IRequest<BaseResponse<string>>
    {

        public Guid BelediyeId { get; set; }
        public Guid KategoriId { get; set; }
        public string Baslik { get; set; }

        public string Aciklama { get; set; }

        public string Adres { get; set; }
        public List<IFormFile> Dosyalar { get; set; }

    }

    public class SikayetEkleCommandHandler : IRequestHandler<SikayetEkle, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<SikayetEkleCommandHandler> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IWebHostEnvironment _env;
        public SikayetEkleCommandHandler(
            IAppDbContext context,
            ILogger<SikayetEkleCommandHandler> logger,
            ICurrentUserService currentUserService, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _currentUserService = currentUserService;
            _env = env;

        }

        public async Task<BaseResponse<string>> Handle(SikayetEkle request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Kullanıcı ID'sini Token'dan Al
            // UserId string gelebilir, Guid'e çevirmemiz lazım.
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Kullanıcı kimliği doğrulanamadı. Lütfen tekrar giriş yapınız.");
            }

            // 2. Kötü Kelime Kontrolü (Hızlı Fail)
            var badWords = await _context.KotuKelimeListesi
                                         .Where(k => !k.silindi)
                                         .Select(k => k.Kelime)
                                         .ToListAsync(cancellationToken);

            if (ContainsBadWords(request.Aciklama, badWords) || ContainsBadWords(request.Baslik, badWords))
            {
                return response.Fail("Şikayetiniz topluluk kurallarına aykırı ifadeler içerdiği için alınmamıştır.");
            }

            // --- TRANSACTION BAŞLAT ---
            await _context.BeginTransactionAsync();

            try
            {
                // 3. ID'yi Elle Oluştur 
                var yeniSikayetId = Guid.NewGuid();
                var baslangicDurumu = SikayetDurumEnum.OnayBekliyor;
                // 4. Şikayet Entity Oluştur
                var sikayet = new Sikayet
                {
                    Id = yeniSikayetId,
                    KategoriId = request.KategoriId,
                    Baslik = request.Baslik,
                    Aciklama = request.Aciklama,
                    BelediyeId = request.BelediyeId,
                    Adres = request.Adres,
                    KullaniciId = currentUserId,
                    Durum = baslangicDurumu.ToString(),
                    OnInceleme = true,
                    OlusturmaTarihi = DateTime.UtcNow
                };

                // 2. DOSYA YÜKLEME İŞLEMİ 
                if (request.Dosyalar != null && request.Dosyalar.Count > 0)
                {
                    // Fiziksel Yol: wwwroot/uploads/sikayetler/{SikayetId}/
                    string anaDizin = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                    string klasorYolu = Path.Combine(anaDizin, "uploads", "sikayetler", yeniSikayetId.ToString());

                    if (!Directory.Exists(klasorYolu))
                    {
                        Directory.CreateDirectory(klasorYolu);
                    }

                    foreach (var dosya in request.Dosyalar)
                    {
                        if (dosya.Length > 0)
                        {
                            string dosyaUzantisi = Path.GetExtension(dosya.FileName);
                            string guvenliDosyaAdi = $"{Guid.NewGuid()}{dosyaUzantisi}";
                            string tamYol = Path.Combine(klasorYolu, guvenliDosyaAdi);

                            using (var stream = new FileStream(tamYol, FileMode.Create))
                            {
                                await dosya.CopyToAsync(stream, cancellationToken);
                            }

                            var ek = new SikayetEk
                            {
                                Id = Guid.NewGuid(),
                                SikayetId = yeniSikayetId,
                                DosyaIsmi = dosya.FileName, 
                                DosyaUrl = $"/uploads/sikayetler/{yeniSikayetId}/{guvenliDosyaAdi}",
                                MimeType = dosya.ContentType, 
                                YuklemeTarihi = DateTime.UtcNow
                            };

                            _context.SikayetEkleri.Add(ek);
                        }
                    }
                }

                // 5. Moderasyon Kuyruğu Entity Oluştur
                var moderasyonKaydi = new ModerasyonKuyrugu
                {
                    Id = Guid.NewGuid(),
                    SikayetId = yeniSikayetId,
                    Neden = "Yeni Şikayet Onayı",
                    Durum = baslangicDurumu.ToString(),
                    IslemTarihi = DateTime.UtcNow
                };

                // C. SİKAYET DURUM TABLOSU
                var durumLog = new SikayetDurum
                {
                    Id = Guid.NewGuid(),
                    SikayetId = yeniSikayetId,
                    Durum = baslangicDurumu,
                    IslemTarihi = DateTime.UtcNow,
                    IslemYapanId = currentUserId,
                    Aciklama = "Şikayet oluşturuldu ve moderatör onayına sunuldu."
                };

                // Hepsini Context'e ekle
                _context.Sikayetler.Add(sikayet);
                _context.ModerasyonKuyrugu.Add(moderasyonKaydi);
                _context.SikayetDurumlari.Add(durumLog);


                // Kaydet
                await _context.SaveChangesAsync(cancellationToken);

                // Commit
                await _context.CommitTransactionAsync();

                return response.Success(sikayet.Id.ToString());
            }
            catch (Exception ex)
            {
                // 9. Hata oldu -> ROLLBACK (Hepsini geri al)
                await _context.RollbackTransactionAsync();

                _logger.LogError($"Şikayet ekleme hatası: {ex.Message}");
                return response.Fail("Şikayet oluşturulurken teknik bir hata meydana geldi.");
            }
        }

        private bool ContainsBadWords(string text, List<string> badWords)
        {
            if (string.IsNullOrWhiteSpace(text) || badWords == null || badWords.Count == 0)
                return false;

            var normalized = new string(text.ToLowerInvariant()
                .Where(c => !char.IsPunctuation(c)).ToArray());

            foreach (var w in badWords)
            {
                var wn = w.ToLowerInvariant().Trim();
                if (string.IsNullOrEmpty(wn)) continue;
                if (normalized.Contains(wn)) return true;
            }
            return false;
        }
    }

    public class SikayetEkleCommandValidator : AbstractValidator<SikayetEkle>
    {
        public SikayetEkleCommandValidator()
        {
            RuleFor(x => x.BelediyeId)
                .NotEmpty().WithMessage("Belediye seçimi zorunludur.");
            RuleFor(x => x.KategoriId)
                .NotEmpty().WithMessage("Kategori seçimi zorunludur.");
 

            RuleFor(x => x.Baslik)
                .NotEmpty().WithMessage("Şikayet başlığı zorunludur.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.Aciklama)
                .NotEmpty().WithMessage("Açıklama alanı zorunludur.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır."); // Çok kısa şikayetleri engellemek için

            RuleFor(x => x.Adres)
                .NotEmpty().WithMessage("Adres bilgisi zorunludur.");
        }

    }
}





