using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeQuest.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Groups",
                newName: "InviteCode");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "InviteCode",
                table: "Groups",
                newName: "Code");
        }
    }
}
