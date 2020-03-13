namespace TripFinder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddEstimatedMinutesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedTime",
                table: "TownsDistances");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedMinutes",
                table: "TownsDistances",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedMinutes",
                table: "TownsDistances");

            migrationBuilder.AddColumn<string>(
                name: "EstimatedTime",
                table: "TownsDistances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
