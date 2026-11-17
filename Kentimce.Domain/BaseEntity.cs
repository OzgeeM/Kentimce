using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class BaseEntity
    {
        public DateTime? eklemeTarihi { get; set; }
        public Guid? ekleyenId { get; set; }
        public DateTime? guncellemeTarihi { get; set; }
        public Guid? guncelleyenId { get; set; }
        public string ip { get; set; }
        public bool silindi { get; set; }
    }
}
