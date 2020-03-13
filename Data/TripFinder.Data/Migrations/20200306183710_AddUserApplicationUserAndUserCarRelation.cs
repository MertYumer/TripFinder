namespace TripFinder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddUserApplicationUserAndUserCarRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarId",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarId",
                table: "User");
        }
    }
}
