using Microsoft.EntityFrameworkCore.Migrations;

namespace TripFinder.Data.Migrations
{
    public partial class AddTripOriginAndDestinationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Trips",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Origin",
                table: "Trips",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Origin",
                table: "Trips");
        }
    }
}
