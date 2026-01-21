using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaycareManager.Migrations
{
    /// <inheritdoc />
    public partial class AddClassroomCapacity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Classrooms",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Classrooms");
        }
    }
}
