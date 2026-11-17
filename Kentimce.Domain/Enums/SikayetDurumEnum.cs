using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Domain.Enums
{
    public enum SikayetDurumEnum
    {
        OnayBekliyor = 0,   // Moderatör Önüne Düşer
        Yayinda = 1,        // Herkes Görür (Vatandaş Etkileşim Verir)
        CozumBekliyor = 2,  // Belediye Personeli Görür
        Cozuldu = 3,        // Kapanmış
        Reddedildi = 4      // Moderatör Reddetti
    }
}
