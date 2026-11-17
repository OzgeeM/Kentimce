using FluentValidation;
using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using Kentimce.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kentimce.Application.Commands.SikayetIslemleri
{
    public class SikayetYorumEkle : IRequest<BaseResponse<string>>
    {
        public Guid SikayetId { get; set; }
        public string Yorum { get; set; }
    }

    public class SikayetYorumEkleCommandHandler : IRequestHandler<SikayetYorumEkle, BaseResponse<string>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SikayetYorumEkleCommandHandler> _logger;

        public SikayetYorumEkleCommandHandler(
            IAppDbContext context,
            ICurrentUserService currentUserService,
            ILogger<SikayetYorumEkleCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<BaseResponse<string>> Handle(SikayetYorumEkle request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();

            // 1. Kullanıcı ID'sini Token'dan al
            if (string.IsNullOrEmpty(_currentUserService.GetUserId().ToString()) || !Guid.TryParse(_currentUserService.GetUserId().ToString(), out Guid currentUserId))
            {
                return response.Fail("Oturum bilgisi geçersiz.");
            }

            // 2. Kötü Kelime Kontrolü (Yorumlarda da olmalı)
            var badWords = await _context.KotuKelimeListesi
                                         .Where(k => !k.silindi)
                                         .Select(k => k.Kelime)
                                         .ToListAsync(cancellationToken);

            if (ContainsBadWords(request.Yorum, badWords))
            {
                return response.Fail("Yorumunuz uygunsuz içerik barındırdığı için eklenemedi.");
            }

            // 3. Şikayeti ve Yorum Yapacak Kullanıcıyı Getir
            var sikayet = await _context.Sikayetler
                                        .FirstOrDefaultAsync(s => s.Id == request.SikayetId, cancellationToken);

            if (sikayet == null)
            {
                return response.Fail("Şikayet bulunamadı.");
            }

            // Yorum yapan kullanıcının rolleri (Belediye Personeli mi diye bakmak için)
            var kullaniciRolleri = await _context.KullaniciRolleri
                                                 .Include(kr => kr.rol)
                                                 .Where(kr => kr.KullaniciId == currentUserId)
                                                 .Select(kr => kr.rol.Isim)
                                                 .ToListAsync(cancellationToken);

            // 4. YETKİ KONTROLÜ 

            bool isOwner = sikayet.KullaniciId == currentUserId; // Şikayeti açan kişi mi?
            bool isPersonnel = kullaniciRolleri.Contains("Personel"); // Belediye personeli mi?

            

            if (!isOwner && !isPersonnel)
            {
                // Ne sahibi ne de personel -> Engel!
                return response.Fail("Bu şikayete sadece şikayet sahibi ve yetkili belediye birimi yorum yapabilir.");
            }

            // 5. Yorumu Kaydet
            var yeniYorum = new SikayetYorum
            {
                Id = Guid.NewGuid(),
                SikayetId = request.SikayetId,
                KullaniciId = currentUserId,
                Yorum = request.Yorum
               
            };

            _context.SikayetYorumlari.Add(yeniYorum);
            await _context.SaveChangesAsync(cancellationToken);

            return response.Success("Yorumunuz başarıyla eklendi.");
        }

        // Kötü kelime metodu 
        private bool ContainsBadWords(string text, List<string> badWords)
        {
            if (string.IsNullOrWhiteSpace(text) || badWords == null || badWords.Count == 0) return false;
            var normalized = new string(text.ToLowerInvariant().Where(c => !char.IsPunctuation(c)).ToArray());
            foreach (var w in badWords)
            {
                var wn = w.ToLowerInvariant().Trim();
                if (string.IsNullOrEmpty(wn)) continue;
                if (normalized.Contains(wn)) return true;
            }
            return false;
        }
    }

    public class SikayetYorumEkleValidator : AbstractValidator<SikayetYorumEkle>
    {
        public SikayetYorumEkleValidator()
        {
            RuleFor(x => x.SikayetId)
                .NotEmpty().WithMessage("Şikayet ID'si boş olamaz.");

            RuleFor(x => x.Yorum)
                .NotEmpty().WithMessage("Yorum metni boş olamaz.")
                .MinimumLength(3).WithMessage("Yorum çok kısa.")
                .MaximumLength(1000).WithMessage("Yorum en fazla 1000 karakter olabilir.");
        }
    }
}
