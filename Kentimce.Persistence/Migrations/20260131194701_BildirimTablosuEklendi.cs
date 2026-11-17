using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kentimce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BildirimTablosuEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bildirimler",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KullaniciId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mesaj = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    OkunduMu = table.Column<bool>(type: "boolean", nullable: false),
                    Tarih = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bildirimler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bildirimler");
        }
    }
}
