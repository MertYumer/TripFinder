namespace TripFinder.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeTripIdOnDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrips_Trips_TripId",
                table: "UserTrips");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrips_Trips_TripId",
                table: "UserTrips",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrips_Trips_TripId",
                table: "UserTrips");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrips_Trips_TripId",
                table: "UserTrips",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");
        }
    }
}
