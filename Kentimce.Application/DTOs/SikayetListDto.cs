using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class SikayetListDto
    {
        public Guid Id { get; set; }
        public string Baslik { get; set; }
        public string Ozet { get; set; } // Açıklamanın ilk 100 karakteri
        public string Adres { get; set; }
        public string Durum { get; set; } // Yayinda, Cozuldu vs.
        public DateTime Tarih { get; set; }
        public string KullaniciAdSoyad { get; set; }
        public string BelediyeAdi { get; set; }
        public string KategoriAdi { get; set; }
        public string KategoriIkon { get; set; } // Frontend'de ikon göstermek için
        public string KategoriRenk { get; set; } // Frontend'de renk göstermek için
        public int BegeniSayisi { get; set; }
        public int YorumSayisi { get; set; }
        public bool BenBegendimMi { get; set; }
    }
}
