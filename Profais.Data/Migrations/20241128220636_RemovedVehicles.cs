using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Profais.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTasks_Vehicles_VehicleId",
                table: "UsersTasks");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersTasks",
                table: "UsersTasks");

            migrationBuilder.DropIndex(
                name: "IX_UsersTasks_VehicleId",
                table: "UsersTasks");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "UsersTasks");

            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Tasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersTasks",
                table: "UsersTasks",
                columns: new[] { "WorkerId", "TaskId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersTasks",
                table: "UsersTasks");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "UsersTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HoursWorked",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersTasks",
                table: "UsersTasks",
                columns: new[] { "WorkerId", "TaskId", "VehicleId" });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                column: "HoursWorked",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 5,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 6,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 7,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 8,
                column: "HoursWorked",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 9,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 10,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 11,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 12,
                column: "HoursWorked",
                value: 14);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 13,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 14,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 15,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 16,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 17,
                column: "HoursWorked",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 18,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 19,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 20,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 21,
                column: "HoursWorked",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 22,
                column: "HoursWorked",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 23,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 24,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 25,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 26,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 27,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 28,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 29,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 30,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 31,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 32,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 33,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 34,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 35,
                column: "HoursWorked",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 36,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 37,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 38,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 39,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 40,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 41,
                column: "HoursWorked",
                value: 7);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 42,
                column: "HoursWorked",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 43,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 44,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 45,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 46,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 47,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 48,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 49,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 50,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 51,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 52,
                column: "HoursWorked",
                value: 18);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 53,
                column: "HoursWorked",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 54,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 55,
                column: "HoursWorked",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 56,
                column: "HoursWorked",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 57,
                column: "HoursWorked",
                value: 20);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 58,
                column: "HoursWorked",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 59,
                column: "HoursWorked",
                value: 10);

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 60,
                column: "HoursWorked",
                value: 7);

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Capacity", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 5, "Toyota Corolla", 0 },
                    { 2, 4, "Honda Civic", 0 },
                    { 3, 4, "Ford Focus", 0 },
                    { 4, 2, "Mercedes-Benz Actros", 1 },
                    { 5, 3, "Volvo FH16", 1 },
                    { 6, 2, "MAN TGX", 1 },
                    { 7, 1, "Hyster H50XT", 2 },
                    { 8, 1, "Toyota 8FGCU25", 2 },
                    { 9, 1, "Caterpillar GP25N", 2 },
                    { 10, 3, "CAT 745C", 3 },
                    { 11, 3, "Komatsu HD785", 3 },
                    { 12, 2, "Volvo A40G", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersTasks_VehicleId",
                table: "UsersTasks",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTasks_Vehicles_VehicleId",
                table: "UsersTasks",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
