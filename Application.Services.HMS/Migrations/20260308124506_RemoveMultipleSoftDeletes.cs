using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Services.HMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMultipleSoftDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "RoomPriceModifiers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Amenities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "RoomPriceModifiers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Amenities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
