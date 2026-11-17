using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class SikayetDetailDto
    {
        public Guid Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Adres { get; set; }
        public string Durum { get; set; }
        public DateTime Tarih { get; set; }

        // Şikayet Eden Bilgisi
        public string KullaniciAdSoyad { get; set; }
        public Guid? KullaniciId { get; set; } // Şikayet sahibi mi kontrolü için lazım

        // Belediye Bilgisi
        public string BelediyeAdi { get; set; }

        // Etkileşim Bilgileri
        public int BegeniSayisi { get; set; }
        public bool BenBegendimMi { get; set; } // UI'da kalbi kırmızı yapmak için

        // İlişkili Veriler
        public List<SikayetYorumDto> Yorumlar { get; set; }
    }
}
