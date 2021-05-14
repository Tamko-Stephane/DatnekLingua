using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DatnekLingua_API.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsCompulsoryToTheApplication = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "NiveauxComprehensions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveauxComprehensions", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "NiveauxEcrits",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveauxEcrits", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "NiveauxParles",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveauxParles", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "UserConfiguredLanguages",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    IsApplicationUserLanguage = table.Column<bool>(nullable: false),
                    LanguageGuid = table.Column<Guid>(nullable: false),
                    NiveauParleGuid = table.Column<Guid>(nullable: false),
                    NiveauEcritGuid = table.Column<Guid>(nullable: false),
                    NiveauComprehensionGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfiguredLanguages", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_UserConfiguredLanguages_Languages_LanguageGuid",
                        column: x => x.LanguageGuid,
                        principalTable: "Languages",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConfiguredLanguages_NiveauxComprehensions_NiveauComprehensionGuid",
                        column: x => x.NiveauComprehensionGuid,
                        principalTable: "NiveauxComprehensions",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConfiguredLanguages_NiveauxEcrits_NiveauEcritGuid",
                        column: x => x.NiveauEcritGuid,
                        principalTable: "NiveauxEcrits",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserConfiguredLanguages_NiveauxParles_NiveauParleGuid",
                        column: x => x.NiveauParleGuid,
                        principalTable: "NiveauxParles",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguredLanguages_LanguageGuid",
                table: "UserConfiguredLanguages",
                column: "LanguageGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguredLanguages_NiveauComprehensionGuid",
                table: "UserConfiguredLanguages",
                column: "NiveauComprehensionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguredLanguages_NiveauEcritGuid",
                table: "UserConfiguredLanguages",
                column: "NiveauEcritGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguredLanguages_NiveauParleGuid",
                table: "UserConfiguredLanguages",
                column: "NiveauParleGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConfiguredLanguages");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "NiveauxComprehensions");

            migrationBuilder.DropTable(
                name: "NiveauxEcrits");

            migrationBuilder.DropTable(
                name: "NiveauxParles");
        }
    }
}
