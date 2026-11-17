using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Bildirim : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid KullaniciId { get; set; } // Kime gidecek?
        public string Mesaj { get; set; }
        public string Link { get; set; }      // Tıklayınca nereye gidecek?
        public bool OkunduMu { get; set; } = false;
        public DateTime Tarih { get; set; } = DateTime.UtcNow;
    }
}
