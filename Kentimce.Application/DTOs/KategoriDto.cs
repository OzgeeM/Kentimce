using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.DTOs
{
    public class KategoriDto
    {
        public Guid Id { get; set; }
        public string Ad { get; set; }
        public string Ikon { get; set; }
        public string RenkKodu { get; set; }
    }
}
