using Microsoft.EntityFrameworkCore.Migrations;

namespace DatnekLingua_API.Data.Migrations
{
    public partial class Add_DefaultValueInstance_ToLevels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultValueInstance",
                table: "NiveauxParles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultValueInstance",
                table: "NiveauxEcrits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDefaultValueInstance",
                table: "NiveauxComprehensions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefaultValueInstance",
                table: "NiveauxParles");

            migrationBuilder.DropColumn(
                name: "IsDefaultValueInstance",
                table: "NiveauxEcrits");

            migrationBuilder.DropColumn(
                name: "IsDefaultValueInstance",
                table: "NiveauxComprehensions");
        }
    }
}
