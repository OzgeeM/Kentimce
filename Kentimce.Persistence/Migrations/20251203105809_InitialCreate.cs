using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kentimce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "iller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Isim = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kotuKelimeListesi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Kelime = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kotuKelimeListesi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "kullanicilar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Telefon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Soyad = table.Column<string>(type: "text", nullable: false),
                    ParolaHash = table.Column<string>(type: "text", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Isim = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ilceler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    IlId = table.Column<Guid>(type: "uuid", nullable: false),
                    Isim = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ilceler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ilceler_iller_IlId",
                        column: x => x.IlId,
                        principalTable: "iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kullaniciRolleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    RolId = table.Column<Guid>(type: "uuid", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kullaniciRolleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_kullaniciRolleri_kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_kullaniciRolleri_roller_RolId",
                        column: x => x.RolId,
                        principalTable: "roller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "belediyeler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    IlId = table.Column<Guid>(type: "uuid", nullable: false),
                    IlceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Isim = table.Column<string>(type: "text", nullable: false),
                    Tip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_belediyeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_belediyeler_ilceler_IlceId",
                        column: x => x.IlceId,
                        principalTable: "ilceler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_belediyeler_iller_IlId",
                        column: x => x.IlId,
                        principalTable: "iller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "sikayetler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: true),
                    BelediyeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Baslik = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    Adres = table.Column<string>(type: "text", nullable: false),
                    Durum = table.Column<string>(type: "text", nullable: false, defaultValue: "Yeni"),
                    OnInceleme = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    GuncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sikayetler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sikayetler_belediyeler_BelediyeId",
                        column: x => x.BelediyeId,
                        principalTable: "belediyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sikayetler_kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "moderasyonKuyrugu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SikayetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Neden = table.Column<string>(type: "text", nullable: false),
                    IslemTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Durum = table.Column<string>(type: "text", nullable: false),
                    IslemiYapan = table.Column<Guid>(type: "uuid", nullable: true),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moderasyonKuyrugu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_moderasyonKuyrugu_kullanicilar_IslemiYapan",
                        column: x => x.IslemiYapan,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_moderasyonKuyrugu_sikayetler_SikayetId",
                        column: x => x.SikayetId,
                        principalTable: "sikayetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sikayetBegenileri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SikayetId = table.Column<Guid>(type: "uuid", nullable: false),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sikayetBegenileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sikayetBegenileri_kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sikayetBegenileri_sikayetler_SikayetId",
                        column: x => x.SikayetId,
                        principalTable: "sikayetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sikayetDurumlari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SikayetId = table.Column<Guid>(type: "uuid", nullable: false),
                    EskiDurum = table.Column<string>(type: "text", nullable: false),
                    YeniDurum = table.Column<string>(type: "text", nullable: false),
                    Aciklama = table.Column<string>(type: "text", nullable: false),
                    DegistirenKullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sikayetDurumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sikayetDurumlari_kullanicilar_DegistirenKullaniciId",
                        column: x => x.DegistirenKullaniciId,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sikayetDurumlari_sikayetler_SikayetId",
                        column: x => x.SikayetId,
                        principalTable: "sikayetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sikayetEkleri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SikayetId = table.Column<Guid>(type: "uuid", nullable: false),
                    DosyaIsmi = table.Column<string>(type: "text", nullable: true),
                    DosyaUrl = table.Column<string>(type: "text", nullable: false),
                    MimeType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    YuklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sikayetEkleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sikayetEkleri_sikayetler_SikayetId",
                        column: x => x.SikayetId,
                        principalTable: "sikayetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sikayetYorumlari",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SikayetId = table.Column<Guid>(type: "uuid", nullable: false),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    Yorum = table.Column<string>(type: "text", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sikayetYorumlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sikayetYorumlari_kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_sikayetYorumlari_sikayetler_SikayetId",
                        column: x => x.SikayetId,
                        principalTable: "sikayetler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_belediyeler_IlceId",
                table: "belediyeler",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_belediyeler_IlId",
                table: "belediyeler",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_ilceler_IlId",
                table: "ilceler",
                column: "IlId");

            migrationBuilder.CreateIndex(
                name: "IX_kotuKelimeListesi_Kelime",
                table: "kotuKelimeListesi",
                column: "Kelime",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kullanicilar_Email",
                table: "kullanicilar",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_kullaniciRolleri_KullaniciId",
                table: "kullaniciRolleri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_kullaniciRolleri_RolId",
                table: "kullaniciRolleri",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_moderasyonKuyrugu_IslemiYapan",
                table: "moderasyonKuyrugu",
                column: "IslemiYapan");

            migrationBuilder.CreateIndex(
                name: "IX_moderasyonKuyrugu_SikayetId",
                table: "moderasyonKuyrugu",
                column: "SikayetId");

            migrationBuilder.CreateIndex(
                name: "IX_roller_Isim",
                table: "roller",
                column: "Isim",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sikayetBegenileri_KullaniciId",
                table: "sikayetBegenileri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetBegenileri_SikayetId_KullaniciId",
                table: "sikayetBegenileri",
                columns: new[] { "SikayetId", "KullaniciId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sikayetDurumlari_DegistirenKullaniciId",
                table: "sikayetDurumlari",
                column: "DegistirenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetDurumlari_SikayetId",
                table: "sikayetDurumlari",
                column: "SikayetId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetEkleri_SikayetId",
                table: "sikayetEkleri",
                column: "SikayetId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetler_BelediyeId",
                table: "sikayetler",
                column: "BelediyeId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetler_KullaniciId",
                table: "sikayetler",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetYorumlari_KullaniciId",
                table: "sikayetYorumlari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_sikayetYorumlari_SikayetId",
                table: "sikayetYorumlari",
                column: "SikayetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kotuKelimeListesi");

            migrationBuilder.DropTable(
                name: "kullaniciRolleri");

            migrationBuilder.DropTable(
                name: "moderasyonKuyrugu");

            migrationBuilder.DropTable(
                name: "sikayetBegenileri");

            migrationBuilder.DropTable(
                name: "sikayetDurumlari");

            migrationBuilder.DropTable(
                name: "sikayetEkleri");

            migrationBuilder.DropTable(
                name: "sikayetYorumlari");

            migrationBuilder.DropTable(
                name: "roller");

            migrationBuilder.DropTable(
                name: "sikayetler");

            migrationBuilder.DropTable(
                name: "belediyeler");

            migrationBuilder.DropTable(
                name: "kullanicilar");

            migrationBuilder.DropTable(
                name: "ilceler");

            migrationBuilder.DropTable(
                name: "iller");
        }
    }
}
