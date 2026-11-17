using Kentimce.Application.Abstractions;
using Kentimce.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;


namespace Kentimce.Persistence.DataContext
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private IDbContextTransaction _currentTransaction;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Il> Iller { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }
        public DbSet<Belediye> Belediyeler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<KullaniciRol> KullaniciRolleri { get; set; }
        public DbSet<Sikayet> Sikayetler { get; set; }
        public DbSet<SikayetEk> SikayetEkleri { get; set; }
        public DbSet<SikayetDurum> SikayetDurumlari { get; set; }
        public DbSet<ModerasyonKuyrugu> ModerasyonKuyrugu { get; set; }
        public DbSet<KotuKelime> KotuKelimeListesi { get; set; }
        public DbSet<SikayetYorum> SikayetYorumlari { get; set; }
        public DbSet<SikayetBegeni> SikayetBegenileri { get; set; }

        public DbSet<SikayetKategori> SikayetKategorileri { get; set; }
        public DbSet<Bildirim> Bildirimler { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
               
                switch (data.State)
                {
                    case EntityState.Added: // Ekleme İşlemi
                        data.Entity.eklemeTarihi = DateTime.UtcNow; // Postgres için UtcNow
                        data.Entity.ekleyenId = _currentUserService.GetUserId();
                        data.Entity.ip = _currentUserService.GetUserIp();
                        data.Entity.silindi = false; 
                        break;

                    case EntityState.Modified: // Güncelleme İşlemi
                        data.Entity.guncellemeTarihi = DateTime.UtcNow;
                        data.Entity.guncelleyenId = _currentUserService.GetUserId();
                        data.Entity.ip = _currentUserService.GetUserIp(); 

                        // Güncelleme sırasında "Ekleme" bilgilerinin bozulmamasını garantiye al
                        data.Property(x => x.eklemeTarihi).IsModified = false;
                        data.Property(x => x.ekleyenId).IsModified = false;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tüm Configuration class'larını otomatik bulur
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;

            // Asıl transaction burada başlıyor
            _currentTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                // Transaction'ı onayla
                await _currentTransaction?.CommitAsync();
            }
            catch
            {
                // Hata olursa geri al
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _currentTransaction?.RollbackAsync();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}
