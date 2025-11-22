using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_TRILHA",
                columns: table => new
                {
                    ID_TRILHA = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NOME_TRILHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    QUANTIDADE_CONTEUDO_TRILHA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_TRILHA_ID_TRILHA_PK", x => x.ID_TRILHA);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NOME_USUARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EMAIL_USUARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SENHA_USUARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DATA_NASCIMENTO_USUARIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_USUARIO_ID_USUARIO_PK", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTEUDO_TRILHA",
                columns: table => new
                {
                    ID_CONTEUDO_TRILHA = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    NOME_CONTEUDO_TRILHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TIPO_CONTEUDO_TRILHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    TEXTO_CONTEUDO_TRILHA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ID_TRILHA = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_CONTEUDO_TRILHA_ID_CONTEUDO_TRILHA_PK", x => x.ID_CONTEUDO_TRILHA);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDO_TRILHA_TB_TRILHA_ID_TRILHA",
                        column: x => x.ID_TRILHA,
                        principalTable: "TB_TRILHA",
                        principalColumn: "ID_TRILHA",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_ENDERECO_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CEP_ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    LOGRADOURO_ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ESTADO_ENDERECO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_ENDERECO_USUARIO_ID_USUARIO_PK", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_ENDERECO_USUARIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_FORMULARIO_PROFISSAO_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RESPOSTA_PERGUNTA_1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_2 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_3 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_4 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_5 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_6 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_7 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_8 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_9 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RESPOSTA_PERGUNTA_10 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PROFISSAO_RECOMENDADA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_FORMULARIO_PROFISSAO_USUARIO_ID_USUARIO_PK", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_FORMULARIO_PROFISSAO_USUARIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_TRILHA_USUARIO",
                columns: table => new
                {
                    ID_TRILHA_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ID_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ID_TRILHA = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    TRILHA_CONCLUIDA_USUARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_TRILHA_USUARIO_ID_TRILHA_USUARIO_PK", x => x.ID_TRILHA_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_TRILHA_USUARIO_TB_TRILHA_ID_TRILHA",
                        column: x => x.ID_TRILHA,
                        principalTable: "TB_TRILHA",
                        principalColumn: "ID_TRILHA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_TRILHA_USUARIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTEUDO_TRILHA_USUARIO",
                columns: table => new
                {
                    ID_CONTEUDO_TRILHA_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    CONTEUDO_TRILHA_CONCLUIDA_USUARIO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ID_USUARIO = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ID_CONTEUDO_TRILHA = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TB_CONTEUDO_TRILHA_USUARIO_ID_CONTEUDO_TRILHA_USUARIO_PK", x => x.ID_CONTEUDO_TRILHA_USUARIO);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDO_TRILHA_USUARIO_TB_CONTEUDO_TRILHA_ID_CONTEUDO_TRILHA",
                        column: x => x.ID_CONTEUDO_TRILHA,
                        principalTable: "TB_CONTEUDO_TRILHA",
                        principalColumn: "ID_CONTEUDO_TRILHA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TB_CONTEUDO_TRILHA_USUARIO_TB_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDO_TRILHA_ID_TRILHA",
                table: "TB_CONTEUDO_TRILHA",
                column: "ID_TRILHA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDO_TRILHA_USUARIO_ID_CONTEUDO_TRILHA",
                table: "TB_CONTEUDO_TRILHA_USUARIO",
                column: "ID_CONTEUDO_TRILHA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTEUDO_TRILHA_USUARIO_ID_USUARIO",
                table: "TB_CONTEUDO_TRILHA_USUARIO",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRILHA_USUARIO_ID_TRILHA",
                table: "TB_TRILHA_USUARIO",
                column: "ID_TRILHA");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TRILHA_USUARIO_ID_USUARIO",
                table: "TB_TRILHA_USUARIO",
                column: "ID_USUARIO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONTEUDO_TRILHA_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_ENDERECO_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_FORMULARIO_PROFISSAO_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_TRILHA_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_CONTEUDO_TRILHA");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_TRILHA");
        }
    }
}
