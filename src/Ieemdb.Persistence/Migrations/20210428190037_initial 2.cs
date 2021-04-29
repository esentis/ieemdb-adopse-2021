using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters");

            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Movies_MovieId1",
                table: "Posters");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Movies_MovieId1",
                table: "Screenshots");

            migrationBuilder.DropIndex(
                name: "IX_Screenshots_MovieId1",
                table: "Screenshots");

            migrationBuilder.DropIndex(
                name: "IX_Posters_MovieId1",
                table: "Posters");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Screenshots");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Posters");

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots");

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "Screenshots",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "Posters",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Screenshots_MovieId1",
                table: "Screenshots",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Posters_MovieId1",
                table: "Posters",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Movies_MovieId1",
                table: "Posters",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Screenshots_Movies_MovieId1",
                table: "Screenshots",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
