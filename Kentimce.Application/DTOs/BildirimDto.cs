using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class BildirimDto
    {
        public Guid Id { get; set; }
        public string Mesaj { get; set; }
        public string Link { get; set; }
        public DateTime Tarih { get; set; }
        public bool OkunduMu { get; set; }
    }
}
