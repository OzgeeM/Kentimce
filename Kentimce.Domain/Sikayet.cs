using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Sikayet : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid? KullaniciId { get; set; }
        public Guid BelediyeId { get; set; }

        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Adres { get; set; }
        public string Durum { get; set; }
        public bool OnInceleme { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }

        public Guid KategoriId { get; set; }
        public virtual SikayetKategori Kategori { get; set; }

        public Kullanici kullanici { get; set; }
        public Belediye belediye { get; set; }

        public List<SikayetEk> ekler { get; set; }
        public List<SikayetDurum> durumGecmisleri { get; set; }
        public List<SikayetYorum> yorumlar { get; set; }
        public List<SikayetBegeni> begeniler { get; set; }
    }

}
