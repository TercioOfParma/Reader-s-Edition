using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReadersEdition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ToSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Definitions",
                columns: table => new
                {
                    DefinitionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WordLanguageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GlossLanguageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Word = table.Column<string>(type: "TEXT", nullable: false),
                    Gloss = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.DefinitionId);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LanguageName = table.Column<string>(type: "TEXT", nullable: false),
                    LanguageCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.LanguageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Definitions");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
