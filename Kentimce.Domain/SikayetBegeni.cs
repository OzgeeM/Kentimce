using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class SikayetBegeni : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SikayetId { get; set; }
        public Guid KullaniciId { get; set; }
        public DateTime Tarih { get; set; }

        public Sikayet sikayet { get; set; }
        public Kullanici kullanici { get; set; }
    }

}
