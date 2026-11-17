using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain
{
    public class Belediye: BaseEntity
    {
        public Guid Id { get; set; }
        public Guid IlId { get; set; }
        public Guid? IlceId { get; set; }
        public string Isim { get; set; }
        public string Tip { get; set; }
        public string Adres { get; set; }
        public string Domain { get; set; }

        public Il il { get; set; }
        public Ilce ilce { get; set; }
        public List<Sikayet> sikayetler { get; set; }
        public ICollection<Kullanici> Personeller { get; set; }
    }
}
