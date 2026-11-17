using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kentimce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class KategoriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KategoriId",
                table: "sikayetler",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SikayetKategorileri",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Ad = table.Column<string>(type: "text", nullable: false),
                    Ikon = table.Column<string>(type: "text", nullable: false),
                    RenkKodu = table.Column<string>(type: "text", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    eklemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ekleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    guncellemeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    guncelleyenId = table.Column<Guid>(type: "uuid", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: false),
                    silindi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SikayetKategorileri", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sikayetler_KategoriId",
                table: "sikayetler",
                column: "KategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_sikayetler_SikayetKategorileri_KategoriId",
                table: "sikayetler",
                column: "KategoriId",
                principalTable: "SikayetKategorileri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sikayetler_SikayetKategorileri_KategoriId",
                table: "sikayetler");

            migrationBuilder.DropTable(
                name: "SikayetKategorileri");

            migrationBuilder.DropIndex(
                name: "IX_sikayetler_KategoriId",
                table: "sikayetler");

            migrationBuilder.DropColumn(
                name: "KategoriId",
                table: "sikayetler");
        }
    }
}
