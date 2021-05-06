using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class IeemdbDBCountryRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryOrigin",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "NormalizedCountry",
                table: "Movies");

            migrationBuilder.AddColumn<long>(
                name: "CountryOriginId",
                table: "Movies",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryOrigin = table.Column<string>(type: "text", nullable: false),
                    NormalizedSearch = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Countries_CountryOriginId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Movies_CountryOriginId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CountryOriginId",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "CountryOrigin",
                table: "Movies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedCountry",
                table: "Movies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
