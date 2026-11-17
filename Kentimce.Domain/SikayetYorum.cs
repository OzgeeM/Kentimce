using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class SikayetYorum : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SikayetId { get; set; }
        public Guid KullaniciId { get; set; }

        public string Yorum { get; set; }

        public Sikayet sikayet { get; set; }
        public Kullanici kullanici { get; set; }
    }

}
