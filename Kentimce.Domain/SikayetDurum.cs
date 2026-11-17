using Kentimce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class SikayetDurum : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SikayetId { get; set; }
        public SikayetDurumEnum Durum { get; set; }
        public string Aciklama { get; set; }

        public Kullanici IslemYapanKullanici { get; set; }
        public Guid IslemYapanId { get; set; }
        public DateTime IslemTarihi { get; set; }

        public Sikayet sikayet { get; set; }
    }

}
