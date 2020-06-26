using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationSystem.Data.Migrations
{
    public partial class changName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Button",
                table: "Sittings");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Sittings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Sittings");

            migrationBuilder.AddColumn<bool>(
                name: "Button",
                table: "Sittings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
