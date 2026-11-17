using Kentimce.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kentimce.Application.Abstractions
{
    public interface IAppDbContext
    {

         DbSet<Il> Iller { get; set; }
         DbSet<Ilce> Ilceler { get; set; }
         DbSet<Belediye> Belediyeler { get; set; }
         DbSet<Kullanici> Kullanicilar { get; set; }
         DbSet<Rol> Roller { get; set; }
         DbSet<KullaniciRol> KullaniciRolleri { get; set; }
         DbSet<Sikayet> Sikayetler { get; set; }
         DbSet<SikayetEk> SikayetEkleri { get; set; }
         DbSet<SikayetDurum> SikayetDurumlari { get; set; }
         DbSet<ModerasyonKuyrugu> ModerasyonKuyrugu { get; set; }
         DbSet<KotuKelime> KotuKelimeListesi { get; set; }
         DbSet<SikayetYorum> SikayetYorumlari { get; set; }
         DbSet<SikayetBegeni> SikayetBegenileri { get; set; }

        DbSet<SikayetKategori> SikayetKategorileri { get; set; }
        DbSet<Bildirim> Bildirimler { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
