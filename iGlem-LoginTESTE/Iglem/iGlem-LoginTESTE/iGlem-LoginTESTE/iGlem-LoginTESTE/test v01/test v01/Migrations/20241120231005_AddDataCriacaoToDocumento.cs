using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_v01.Migrations
{
    /// <inheritdoc />
    public partial class AddDataCriacaoToDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "conteudodocumento",
                table: "documento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "datacriacao",
                table: "documento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "conteudodocumento",
                table: "documento");

            migrationBuilder.DropColumn(
                name: "datacriacao",
                table: "documento");
        }
    }
}
