using Kentimce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class SikayetEkResponse
    {
        public Guid Id { get; set; }
        public string DosyaIsmi { get; set; }
        public string DosyaUrl { get; set; }
        public string MimeType { get; set; }
        public DateTime YuklemeTarihi { get; set; }

    }
}
