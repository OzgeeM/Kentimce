using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Kullanici : BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Tc { get; set; }
        public string ParolaHash { get; set; }
        public DateTime OlusturmaTarihi { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? DogrulamaKodu { get; set; }
        public bool EmailDogrulandiMi { get; set; } = false; 
        public DateTime? DogrulamaKoduGecerlilikSuresi { get; set; }

        public Guid? BelediyeId { get; set; }

        public Belediye Belediye { get; set; }

        public List<KullaniciRol> roller { get; set; }

        public List<Sikayet> sikayetler { get; set; }
    }

}
