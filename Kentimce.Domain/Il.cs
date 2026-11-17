using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Il: BaseEntity
    {
        public Guid Id { get; set; }
        public string Isim { get; set; }
        public List<Ilce> ilceler { get; set; }
        public List<Belediye> belediyeler { get; set; }
    }
}
