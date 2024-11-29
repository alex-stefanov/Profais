using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profais.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    ContributerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.ContributerId, x.ProfProjectId });
                    table.ForeignKey(
                        name: "FK_UserProjects_AspNetUsers_ContributerId",
                        column: x => x.ContributerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_ProfProjectId",
                        column: x => x.ProfProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_ProfProjectId",
                table: "UserProjects",
                column: "ProfProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProjects");
        }
    }
}
