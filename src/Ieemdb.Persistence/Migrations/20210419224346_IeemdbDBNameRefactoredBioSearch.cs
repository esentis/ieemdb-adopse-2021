using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class IeemdbDBNameRefactoredBioSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedFirstName",
                table: "Writers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedLastName",
                table: "Writers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedFirstName",
                table: "Directors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedLastName",
                table: "Directors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedFirstName",
                table: "Actors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedLastName",
                table: "Actors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedFirstName",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "NormalizedLastName",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "NormalizedFirstName",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "NormalizedLastName",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "NormalizedFirstName",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "NormalizedLastName",
                table: "Actors");
        }
    }
}
