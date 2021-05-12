using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class Updateforsinglewatchlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlist_AspNetUsers_UserId",
                table: "Watchlist");

            migrationBuilder.DropTable(
                name: "MovieWatchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Watchlist",
                table: "Watchlist");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Watchlist");

            migrationBuilder.RenameTable(
                name: "Watchlist",
                newName: "Watchlists");

            migrationBuilder.RenameIndex(
                name: "IX_Watchlist_UserId",
                table: "Watchlists",
                newName: "IX_Watchlists_UserId");

            migrationBuilder.AddColumn<long>(
                name: "MovieId",
                table: "Watchlists",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Watchlists",
                table: "Watchlists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_MovieId",
                table: "Watchlists",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_AspNetUsers_UserId",
                table: "Watchlists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlists_Movies_MovieId",
                table: "Watchlists",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_AspNetUsers_UserId",
                table: "Watchlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Watchlists_Movies_MovieId",
                table: "Watchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Watchlists",
                table: "Watchlists");

            migrationBuilder.DropIndex(
                name: "IX_Watchlists_MovieId",
                table: "Watchlists");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Watchlists");

            migrationBuilder.RenameTable(
                name: "Watchlists",
                newName: "Watchlist");

            migrationBuilder.RenameIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlist",
                newName: "IX_Watchlist_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Watchlist",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Watchlist",
                table: "Watchlist",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MovieWatchlists",
                columns: table => new
                {
                    MovieId = table.Column<long>(type: "bigint", nullable: false),
                    WatchlistId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieWatchlists", x => new { x.MovieId, x.WatchlistId });
                    table.ForeignKey(
                        name: "FK_MovieWatchlists_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovieWatchlists_Watchlist_WatchlistId",
                        column: x => x.WatchlistId,
                        principalTable: "Watchlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieWatchlists_WatchlistId",
                table: "MovieWatchlists",
                column: "WatchlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchlist_AspNetUsers_UserId",
                table: "Watchlist",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
