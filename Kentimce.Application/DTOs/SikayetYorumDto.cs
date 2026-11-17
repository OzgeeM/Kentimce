using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class SikayetYorumDto
    {
        public Guid Id { get; set; }
        public string Yorum { get; set; }
        public string YazanAdSoyad { get; set; }
        public DateTime? Tarih { get; set; }
        public bool YetkiliYorumuMu { get; set; } // Yazan kişi belediye personeli mi?
    }
}
