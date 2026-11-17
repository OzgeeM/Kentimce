using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class BelediyeDto
    {
        public Guid Id { get; set; }
        public string Isim { get; set; }
        public string Domain { get; set; } // Frontend'de uyarı göstermek içşn
        public string Tip { get; set; } // Büyükşehir, İlçe vs. ayrımı için
    }
}
