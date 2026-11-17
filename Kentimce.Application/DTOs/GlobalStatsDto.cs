using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class GlobalStatsDto
    {
        public int Toplam { get; set; }
        public int Cozulen { get; set; }
        public int Bekleyen { get; set; } // Yayında olup çözüm bekleyenler
    }
}
