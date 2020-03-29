namespace TripFinder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeImageUrlColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
