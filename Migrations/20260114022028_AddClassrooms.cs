using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaycareManager.Migrations
{
    /// <inheritdoc />
    public partial class AddClassrooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassroomId",
                table: "Children",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassroomNumber = table.Column<string>(type: "TEXT", nullable: false),
                    MinAgeMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxAgeMonths = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Children_ClassroomId",
                table: "Children",
                column: "ClassroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_Classrooms_ClassroomId",
                table: "Children",
                column: "ClassroomId",
                principalTable: "Classrooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_Classrooms_ClassroomId",
                table: "Children");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Children_ClassroomId",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "ClassroomId",
                table: "Children");
        }
    }
}
