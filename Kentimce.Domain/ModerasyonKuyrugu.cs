using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class ModerasyonKuyrugu : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid SikayetId { get; set; }

        public string Neden { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public string Durum { get; set; }
        public Guid? IslemiYapan { get; set; }

        public Kullanici? islemiYapanKullanici { get; set; }

        public Sikayet sikayet { get; set; }
    }

}
