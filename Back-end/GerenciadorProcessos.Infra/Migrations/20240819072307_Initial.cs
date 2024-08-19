using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorProcessos.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    AreaResponsavel = table.Column<string>(type: "text", nullable: false),
                    Ferramenta = table.Column<string>(type: "text", nullable: true),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProcessoPaiId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processos_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Processos_Processos_ProcessoPaiId",
                        column: x => x.ProcessoPaiId,
                        principalTable: "Processos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Processos_AreaId",
                table: "Processos",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Processos_ProcessoPaiId",
                table: "Processos",
                column: "ProcessoPaiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Processos");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
