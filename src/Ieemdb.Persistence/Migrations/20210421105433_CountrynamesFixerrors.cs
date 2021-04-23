using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class CountrynamesFixerrors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Countries_CountryOriginId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CountryOriginId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CountryOriginId",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountryOriginId",
                table: "Movies",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_CountryOriginId",
                table: "Movies",
                column: "CountryOriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Countries_CountryOriginId",
                table: "Movies",
                column: "CountryOriginId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
