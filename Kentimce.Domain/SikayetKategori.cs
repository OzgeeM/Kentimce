using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class SikayetKategori : BaseEntity 
    {
        public Guid Id { get; set; }
        public string Ad { get; set; }        // Örn: Park ve Bahçe
        public string Ikon { get; set; }      // Örn: ri-tree-line (React remix icon kodu)
        public string RenkKodu { get; set; }  // Örn: success, warning, info (Bootstrap classları)
        public bool AktifMi { get; set; } = true;

        // İlişkiler
        public ICollection<Sikayet> Sikayetler { get; set; }
    }
}
