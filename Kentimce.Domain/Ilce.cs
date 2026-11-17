using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Ilce : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid IlId { get; set; }
        public string Isim { get; set; }

        public Il il { get; set; }
        public List<Belediye> belediyeler { get; set; }
    }
}
