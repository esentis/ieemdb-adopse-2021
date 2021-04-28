// <auto-generated />

using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class Removesredundantkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActors_Movies_MovieId1",
                table: "MovieActors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCountry_Movies_MovieId1",
                table: "MovieCountry");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId1",
                table: "MovieDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieWriters_Movies_MovieId1",
                table: "MovieWriters");

            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots");

            migrationBuilder.DropIndex(
                name: "IX_MovieWriters_MovieId1",
                table: "MovieWriters");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieDirectors_MovieId1",
                table: "MovieDirectors");

            migrationBuilder.DropIndex(
                name: "IX_MovieCountry_MovieId1",
                table: "MovieCountry");

            migrationBuilder.DropIndex(
                name: "IX_MovieActors_MovieId1",
                table: "MovieActors");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieWriters");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieDirectors");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieCountry");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieActors");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "Screenshots",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "Posters",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters",
                column: "MovieId",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Movies_MovieId",
                table: "Posters");

            migrationBuilder.DropForeignKey(
                name: "FK_Screenshots_Movies_MovieId",
                table: "Screenshots");

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "Screenshots",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "Posters",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "MovieWriters",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "MovieGenres",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "MovieDirectors",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "MovieCountry",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "MovieActors",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieWriters_MovieId1",
                table: "MovieWriters",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieDirectors_MovieId1",
                table: "MovieDirectors",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCountry_MovieId1",
                table: "MovieCountry",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieActors_MovieId1",
                table: "MovieActors",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieActors_Movies_MovieId1",
                table: "MovieActors",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCountry_Movies_MovieId1",
                table: "MovieCountry",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId1",
                table: "MovieDirectors",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieWriters_Movies_MovieId1",
                table: "MovieWriters",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}