using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaycareManager.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Children",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Children");
        }
    }
}
