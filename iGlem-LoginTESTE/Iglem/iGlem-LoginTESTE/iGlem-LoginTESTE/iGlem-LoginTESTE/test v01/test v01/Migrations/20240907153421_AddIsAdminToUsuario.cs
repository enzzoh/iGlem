using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_v01.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAdminToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "localizacao",
                columns: table => new
                {
                    id_localizacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paragrafo = table.Column<int>(type: "int", nullable: true),
                    pagina = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__localiza__3EE0C968DBF23C8D", x => x.id_localizacao);
                });

            migrationBuilder.CreateTable(
                name: "LocalizacaoPalavraChave",
                columns: table => new
                {
                    IdLocalizacao = table.Column<int>(type: "int", nullable: false),
                    IdPalavraChave = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizacaoPalavraChave", x => new { x.IdLocalizacao, x.IdPalavraChave });
                });

            migrationBuilder.CreateTable(
                name: "palavra_chave",
                columns: table => new
                {
                    id_palavra_chave = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    palavra = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__palavra___79A814338B0DFD39", x => x.id_palavra_chave);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    idusuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    emailusuario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    senhausuario = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    recmail = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    nomeusuario = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    telefoneusuario = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuario__080A9743E8FE2A70", x => x.idusuario);
                });

            migrationBuilder.CreateTable(
                name: "palavra_chave_localizacao",
                columns: table => new
                {
                    id_palavra_chave = table.Column<int>(type: "int", nullable: false),
                    id_localizacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__palavra___EA4618A5BD4D02FD", x => new { x.id_palavra_chave, x.id_localizacao });
                    table.ForeignKey(
                        name: "fk_localizacao",
                        column: x => x.id_localizacao,
                        principalTable: "localizacao",
                        principalColumn: "id_localizacao");
                    table.ForeignKey(
                        name: "fk_palavra_chave_localizacao",
                        column: x => x.id_palavra_chave,
                        principalTable: "palavra_chave",
                        principalColumn: "id_palavra_chave");
                });

            migrationBuilder.CreateTable(
                name: "documento",
                columns: table => new
                {
                    documentoid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    caminhodocumento = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    documentonome = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    idusuario = table.Column<int>(type: "int", nullable: true),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__document__69A574B52B997C43", x => x.documentoid);
                    table.ForeignKey(
                        name: "fk_usuario_documento",
                        column: x => x.idusuario,
                        principalTable: "usuario",
                        principalColumn: "idusuario");
                });

            migrationBuilder.CreateTable(
                name: "documento_palavra_chave",
                columns: table => new
                {
                    id_documento = table.Column<int>(type: "int", nullable: false),
                    id_palavra_chave = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__document__7AB466A67BC07A39", x => new { x.id_documento, x.id_palavra_chave });
                    table.ForeignKey(
                        name: "fk_documento",
                        column: x => x.id_documento,
                        principalTable: "documento",
                        principalColumn: "documentoid");
                    table.ForeignKey(
                        name: "fk_palavra_chave",
                        column: x => x.id_palavra_chave,
                        principalTable: "palavra_chave",
                        principalColumn: "id_palavra_chave");
                });

            migrationBuilder.CreateIndex(
                name: "IX_documento_idusuario",
                table: "documento",
                column: "idusuario");

            migrationBuilder.CreateIndex(
                name: "IX_documento_palavra_chave_id_palavra_chave",
                table: "documento_palavra_chave",
                column: "id_palavra_chave");

            migrationBuilder.CreateIndex(
                name: "IX_palavra_chave_localizacao_id_localizacao",
                table: "palavra_chave_localizacao",
                column: "id_localizacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documento_palavra_chave");

            migrationBuilder.DropTable(
                name: "LocalizacaoPalavraChave");

            migrationBuilder.DropTable(
                name: "palavra_chave_localizacao");

            migrationBuilder.DropTable(
                name: "documento");

            migrationBuilder.DropTable(
                name: "localizacao");

            migrationBuilder.DropTable(
                name: "palavra_chave");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
