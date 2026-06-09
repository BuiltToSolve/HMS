using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Services.HMS.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomGalleryDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "RoomGalleries");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "RoomGalleries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
