using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Rol : BaseEntity
    {
        public Guid Id { get; set; }
        public string Isim { get; set; }

        public List<KullaniciRol> kullaniciRolleri { get; set; }
    }

}
