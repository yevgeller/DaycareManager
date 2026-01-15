using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DaycareManager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChildParentIdAddManyMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_Parents_ParentId",
                table: "Children");

            migrationBuilder.DropIndex(
                name: "IX_Children_ParentId",
                table: "Children");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Children");

            migrationBuilder.CreateTable(
                name: "ChildParent",
                columns: table => new
                {
                    ChildrenId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildParent", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_ChildParent_Children_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "Children",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChildParent_Parents_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChildParent_ParentsId",
                table: "ChildParent",
                column: "ParentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildParent");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Children",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Children_ParentId",
                table: "Children",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_Parents_ParentId",
                table: "Children",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id");
        }
    }
}
