using FluentValidation;
using Kentimce.Application.Abstractions;
using Kentimce.Application.Enums;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;

namespace Kentimce.Application.Commands.RegisterIslemleri
{
    public class Kayit : IRequest<BaseResponse<string>>
    {

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Sifre { get; set; }
        public string Tc { get; set; }
        public Guid? BelediyeId { get; set; }
        public KullaniciTipi KullaniciTipi { get; set; }


    }

    public class KayitCommandHandler : IRequestHandler<Kayit, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<KayitCommandHandler> _logger;
        private readonly IEmailService _emailService;
        public KayitCommandHandler(
            IAppDbContext context,
            ILogger<KayitCommandHandler> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;

        }

        public async Task<BaseResponse<string>> Handle(Kayit request, CancellationToken cancellationToken)
        {

            var response = new BaseResponse<string>();


            try
            {
                request.Email = request.Email.Trim().ToLower();

                // 1. KULLANICI KONTROLÜ (GÜNCELLENDİ)
                var mevcutKullanici = await _context.Kullanicilar
                    .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
                if (mevcutKullanici != null)
                {
                    // Eğer daha önce doğrulamışsa, gerçekten bu hesap kullanımda demektir.
                    if (mevcutKullanici.EmailDogrulandiMi)
                    {
                        return response.Fail("Bu e-posta adresi zaten kullanımda ve doğrulanmış. Lütfen giriş yapınız.");
                    }

                    // Kullanıcı var AMA doğrulanmamış. Demek ki yarım kalmış.
                    // Bu kullanıcıyı silmek yerine GÜNCELLEYECEĞİZ ve yeni kod göndereceğiz.
                    // Aşağıdaki akışta "yeniKullanici" yerine "mevcutKullanici"yı kullanacağız.
                }


                if (request.KullaniciTipi == KullaniciTipi.BelediyePersoneli)
                {
                    // 1. Seçim Yapmış mı?
                    if (request.BelediyeId == null)
                    {
                        return response.Fail("Belediye personeli kaydı için bağlı olduğunuz belediyeyi seçmelisiniz.");
                    }

                    // 2. Seçilen Belediye Var mı?
                    var secilenBelediye = await _context.Belediyeler.FindAsync(request.BelediyeId);
                    if (secilenBelediye == null)
                    {
                        return response.Fail("Seçilen belediye sistemde bulunamadı.");
                    }

                    // 3. KRİTİK GÜVENLİK KONTROLÜ: Mail Domain'i ile Belediye Uyuşuyor mu?
                    if (!string.IsNullOrEmpty(secilenBelediye.Domain) && !request.Email.EndsWith(secilenBelediye.Domain))
                    {
                        return response.Fail($"Girdiğiniz e-posta adresi, seçtiğiniz {secilenBelediye.Isim} kurumsal domain adresi ({secilenBelediye.Domain}) ile uyuşmuyor.");
                    }
                }
                else
                {
                    // Vatandaş ise BelediyeId null olmalı, dolu geldiyse bile boşaltalım.
                    request.BelediyeId = null;
                }

                // 3. Şifreleme ve Kod Üretme
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Sifre);

                // 6 Haneli Rastgele Sayı
                var dogrulamaKodu = new Random().Next(100000, 999999).ToString();

              

                await _context.BeginTransactionAsync();

                try
                {
                    if (mevcutKullanici != null)
                    {
                        // --- SENARYO A: YARIM KALAN KAYDI GÜNCELLE ---
                        mevcutKullanici.Ad = request.Ad;
                        mevcutKullanici.Soyad = request.Soyad;
                        mevcutKullanici.Telefon = request.Telefon;
                        mevcutKullanici.Tc = request.Tc;
                        mevcutKullanici.ParolaHash = passwordHash; // Yeni şifre
                        mevcutKullanici.BelediyeId = request.BelediyeId;
                        mevcutKullanici.DogrulamaKodu = dogrulamaKodu; // Yeni kod
                        mevcutKullanici.DogrulamaKoduGecerlilikSuresi = DateTime.UtcNow.AddMinutes(5);

                        _context.Kullanicilar.Update(mevcutKullanici);
                    }
                    else
                    {
                        // 4. Entity Oluşturma
                        var yeniKullanici = new Kullanici
                        {
                            Id = Guid.NewGuid(),
                            Ad = request.Ad,
                            Soyad = request.Soyad,
                            Email = request.Email,
                            Telefon = request.Telefon,
                            Tc = request.Tc,
                            ParolaHash = passwordHash,

                            // Doğrulama Bilgileri
                            DogrulamaKodu = dogrulamaKodu,
                            EmailDogrulandiMi = false, // Henüz doğrulanmadı
                            DogrulamaKoduGecerlilikSuresi = DateTime.UtcNow.AddMinutes(5)
                        };
                        string atanacakRolIsmi = request.KullaniciTipi == KullaniciTipi.BelediyePersoneli ? "Personel" : "Vatandas";
                        var rol = await _context.Roller.FirstOrDefaultAsync(r => r.Isim == atanacakRolIsmi);
                        if (rol != null)
                        {
                            yeniKullanici.roller = new List<KullaniciRol>
                    {
                        new KullaniciRol { Id = Guid.NewGuid(), KullaniciId = yeniKullanici.Id, RolId = rol.Id }
                    };
                        }
                        // 1. Veritabanına Ekle (Henüz kalıcı değil)
                        await _context.Kullanicilar.AddAsync(yeniKullanici, cancellationToken);
                    }
                    await _context.SaveChangesAsync(cancellationToken);

                    // 3. Her şey yolunda, veritabanı işlemini onayla (Commit)
                    await _context.CommitTransactionAsync();

                }
                catch (Exception ex)
                {
                    // 4. Mail gönderirken veya kaydederken hata oldu.
                    // Veritabanına eklenen o kullanıcıyı geri al (Sil).
                    await _context.RollbackTransactionAsync();

                    _logger.LogError($"Kayıt işlemi sırasında hata: {ex.Message}");

                    return response.Fail("Kayıt işlemi sırasında bir hata oluştu. Lütfen mail adresinizi kontrol edip tekrar deneyiniz.");
                }

                try
                {
                    string emailBody = $"Merhaba {request.Ad},<br>Kayıt kodunuz: <b>{dogrulamaKodu}</b>";
                    await _emailService.SendEmailAsync(request.Email, "Doğrulama Kodu", emailBody);

                    return response.Success("Kayıt başarılı! Mail adresinize gelen kodu giriniz.");
                }
                catch (Exception mailEx)
                {
                    // Mail atılamadı ama kayıt başarılı, loglayalım
                    _logger.LogError($"Kullanıcı kaydedildi ama mail atılamadı: {mailEx.Message}");
                    return response.Success("Kayıt başarılı ancak doğrulama kodu gönderilemedi. Lütfen giriş ekranından 'Kod Tekrar Gönder' işlemini deneyiniz.");
                }
            }
            catch (System.Exception ex)
            {

                _logger.LogError($@"KayitCommandHandler metodu hata üretti.
{request.ToJson(Newtonsoft.Json.Formatting.Indented)}
Exception : {ex}");
                return response.Fail($"İşlem başarısız. Hata: {ex.Message}");
            }

        }
    }


    public class KayitCommandValidator : AbstractValidator<Kayit>
    {
        public KayitCommandValidator()
        {
            RuleFor(x => x.Ad)
                .NotEmpty().WithMessage("Ad alanı boş geçilemez."); 

            RuleFor(x => x.Soyad)
                .NotEmpty().WithMessage("Soyad alanı boş geçilemez.");

          
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email giriniz.");

            RuleFor(x => x.Sifre)
                .NotEmpty().WithMessage("Sifre alanı boş geçilemez.");

            
            RuleFor(x => x.Tc)
               .Length(11).When(x => !string.IsNullOrEmpty(x.Tc)).WithMessage("TC 11 haneli olmalıdır.");
        }
    }
}





