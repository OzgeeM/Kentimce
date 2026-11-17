using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kentimce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class yeni2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sikayetDurumlari_kullanicilar_DegistirenKullaniciId",
                table: "sikayetDurumlari");

            migrationBuilder.DropColumn(
                name: "EskiDurum",
                table: "sikayetDurumlari");

            migrationBuilder.DropColumn(
                name: "YeniDurum",
                table: "sikayetDurumlari");

            migrationBuilder.RenameColumn(
                name: "Tarih",
                table: "sikayetDurumlari",
                newName: "IslemTarihi");

            migrationBuilder.RenameColumn(
                name: "DegistirenKullaniciId",
                table: "sikayetDurumlari",
                newName: "IslemYapanId");

            migrationBuilder.RenameIndex(
                name: "IX_sikayetDurumlari_DegistirenKullaniciId",
                table: "sikayetDurumlari",
                newName: "IX_sikayetDurumlari_IslemYapanId");

            migrationBuilder.AddColumn<int>(
                name: "Durum",
                table: "sikayetDurumlari",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BelediyeId",
                table: "kullanicilar",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Domain",
                table: "belediyeler",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_kullanicilar_BelediyeId",
                table: "kullanicilar",
                column: "BelediyeId");

            migrationBuilder.AddForeignKey(
                name: "FK_kullanicilar_belediyeler_BelediyeId",
                table: "kullanicilar",
                column: "BelediyeId",
                principalTable: "belediyeler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_sikayetDurumlari_kullanicilar_IslemYapanId",
                table: "sikayetDurumlari",
                column: "IslemYapanId",
                principalTable: "kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kullanicilar_belediyeler_BelediyeId",
                table: "kullanicilar");

            migrationBuilder.DropForeignKey(
                name: "FK_sikayetDurumlari_kullanicilar_IslemYapanId",
                table: "sikayetDurumlari");

            migrationBuilder.DropIndex(
                name: "IX_kullanicilar_BelediyeId",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "sikayetDurumlari");

            migrationBuilder.DropColumn(
                name: "BelediyeId",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "Domain",
                table: "belediyeler");

            migrationBuilder.RenameColumn(
                name: "IslemYapanId",
                table: "sikayetDurumlari",
                newName: "DegistirenKullaniciId");

            migrationBuilder.RenameColumn(
                name: "IslemTarihi",
                table: "sikayetDurumlari",
                newName: "Tarih");

            migrationBuilder.RenameIndex(
                name: "IX_sikayetDurumlari_IslemYapanId",
                table: "sikayetDurumlari",
                newName: "IX_sikayetDurumlari_DegistirenKullaniciId");

            migrationBuilder.AddColumn<string>(
                name: "EskiDurum",
                table: "sikayetDurumlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YeniDurum",
                table: "sikayetDurumlari",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_sikayetDurumlari_kullanicilar_DegistirenKullaniciId",
                table: "sikayetDurumlari",
                column: "DegistirenKullaniciId",
                principalTable: "kullanicilar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
