using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class KullaniciRol : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid KullaniciId { get; set; }
        public Guid RolId { get; set; }

        public Kullanici kullanici { get; set; }
        public Rol rol { get; set; }
    }

}
