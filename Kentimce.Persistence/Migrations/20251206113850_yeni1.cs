using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kentimce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class yeni1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DogrulamaKodu",
                table: "kullanicilar",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DogrulamaKoduGecerlilikSuresi",
                table: "kullanicilar",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailDogrulandiMi",
                table: "kullanicilar",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogrulamaKodu",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "DogrulamaKoduGecerlilikSuresi",
                table: "kullanicilar");

            migrationBuilder.DropColumn(
                name: "EmailDogrulandiMi",
                table: "kullanicilar");
        }
    }
}
