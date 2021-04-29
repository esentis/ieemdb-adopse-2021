using Microsoft.EntityFrameworkCore.Migrations;

namespace Esentis.Ieemdb.Persistence.Migrations
{
    public partial class AddedReviewonRatingmodeltry01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Ratings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Ratings");
        }
    }
}
