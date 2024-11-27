using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profais.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "WorkerRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "SpecialistRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "WorkerRequests");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "SpecialistRequests");
        }
    }
}
