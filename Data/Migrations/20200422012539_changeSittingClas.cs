using Microsoft.EntityFrameworkCore.Migrations;

namespace ReservationSystem.Data.Migrations
{
    public partial class changeSittingClas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Sittings");

            migrationBuilder.AddColumn<int>(
                name: "SittingId",
                table: "SittingTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SittingTypeId",
                table: "Sittings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SittingTypes_SittingId",
                table: "SittingTypes",
                column: "SittingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SittingId",
                table: "Reservations",
                column: "SittingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Sittings_SittingId",
                table: "Reservations",
                column: "SittingId",
                principalTable: "Sittings",
                principalColumn: "SittingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SittingTypes_Sittings_SittingId",
                table: "SittingTypes",
                column: "SittingId",
                principalTable: "Sittings",
                principalColumn: "SittingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Sittings_SittingId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_SittingTypes_Sittings_SittingId",
                table: "SittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_SittingTypes_SittingId",
                table: "SittingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SittingId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SittingId",
                table: "SittingTypes");

            migrationBuilder.DropColumn(
                name: "SittingTypeId",
                table: "Sittings");

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Sittings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Sittings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
