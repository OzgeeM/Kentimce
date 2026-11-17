using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class KotuKelime : BaseEntity
    {
        public Guid Id { get; set; }
        public string Kelime { get; set; }
      
    }

}
