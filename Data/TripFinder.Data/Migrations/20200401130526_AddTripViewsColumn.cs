using Microsoft.EntityFrameworkCore.Migrations;

namespace TripFinder.Data.Migrations
{
    public partial class AddTripViewsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Trips",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Trips");
        }
    }
}
