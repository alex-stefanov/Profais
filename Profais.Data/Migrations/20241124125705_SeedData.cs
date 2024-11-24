using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Profais.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectsRequests_ProfProjectRequestId",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vehicles",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "nvarchar(65)",
                maxLength: 65,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Projects",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "ProfProjectRequestId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "SpecialistRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ProfixId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialistRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ProfixId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerRequests", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "Id", "Name", "UsedForId" },
                values: new object[,]
                {
                    { 1, "Activated Carbon Filter", 0 },
                    { 2, "Sand Filter", 0 },
                    { 3, "UV Sterilizer", 0 },
                    { 4, "Reverse Osmosis Membrane", 0 },
                    { 5, "Pre-Filter Cartridge", 0 },
                    { 6, "Sediment Filter", 0 },
                    { 7, "Chemical Dosing Pump", 0 },
                    { 8, "Drip Irrigation Pipe", 1 },
                    { 9, "Sprinkler Head", 1 },
                    { 10, "Irrigation Valve", 1 },
                    { 11, "PVC Piping", 1 },
                    { 12, "Control Timer", 1 },
                    { 13, "Filter Unit", 1 },
                    { 14, "Fertilizer Injector", 1 },
                    { 15, "Pressure Tank", 2 },
                    { 16, "Pressure Gauge", 2 },
                    { 17, "Water Pump", 2 },
                    { 18, "Expansion Vessel", 2 },
                    { 19, "Non-Return Valve", 2 },
                    { 20, "Pressure Switch", 2 },
                    { 21, "Control Panel", 2 },
                    { 22, "Water Tank", 3 },
                    { 23, "Tank Lid", 3 },
                    { 24, "Overflow Pipe", 3 },
                    { 25, "Level Sensor", 3 },
                    { 26, "Tank Stand", 3 },
                    { 27, "Manhole Cover", 3 },
                    { 28, "Drainage Outlet", 3 },
                    { 29, "HDPE Pipe", 4 },
                    { 30, "Flow Meter", 4 },
                    { 31, "Gate Valve", 4 },
                    { 32, "T-Joint", 4 },
                    { 33, "Compression Fittings", 4 },
                    { 34, "Air Release Valve", 4 },
                    { 35, "PVC Coupling", 4 },
                    { 36, "Perforated Pipe", 5 },
                    { 37, "Catch Basin", 5 },
                    { 38, "Gravel Filter", 5 },
                    { 39, "Drain Mat", 5 },
                    { 40, "Sump Pump", 5 },
                    { 41, "Drainage Grate", 5 },
                    { 42, "Concrete Pipe", 5 },
                    { 43, "Submersible Pump", 6 },
                    { 44, "Pump Motor", 6 },
                    { 45, "Impeller", 6 },
                    { 46, "Pump Housing", 6 },
                    { 47, "Suction Pipe", 6 },
                    { 48, "Pump Shaft", 6 },
                    { 49, "Mechanical Seal", 6 },
                    { 50, "Rubber Gasket", 7 },
                    { 51, "Thread Seal Tape", 7 },
                    { 52, "Clamp", 7 },
                    { 53, "Bracket", 7 },
                    { 54, "Fasteners", 7 },
                    { 55, "Adhesive", 7 },
                    { 56, "Sealant", 7 }
                });

            migrationBuilder.InsertData(
                table: "Penalties",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "The person didn't arrive on time.", "Late for work" },
                    { 2, "The assigned task was not completed on time.", "Missed deadline" },
                    { 3, "The person exhibited inappropriate behavior at the workplace.", "Unprofessional behavior" },
                    { 4, "The person was absent without prior approval.", "Unauthorized absence" },
                    { 5, "The task assigned was not completed as required.", "Incomplete work" },
                    { 6, "Company property was damaged due to negligence.", "Damage to company property" },
                    { 7, "Instructions were not adhered to as directed.", "Failure to follow instructions" },
                    { 8, "The person disregarded workplace safety guidelines.", "Violation of safety protocols" },
                    { 9, "Resources were used inefficiently or inappropriately.", "Improper use of resources" },
                    { 10, "The overall performance fell below the acceptable standards.", "Poor performance" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AbsoluteAddress", "IsCompleted", "ProfProjectRequestId", "Scheme", "Title" },
                values: new object[,]
                {
                    { 1, "Ulitsa Tsarigradsko Shose 125, Sofia, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Sofia Central Water Supply" },
                    { 2, "Ulitsa Ivan Vazov 17, Plovdiv, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Plovdiv Irrigation Upgrade" },
                    { 3, "Ulitsa Levski 55, Varna, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Varna Wastewater Management" },
                    { 4, "Ulitsa Aleksandrovska 10, Burgas, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Burgas Hydrophore System" },
                    { 5, "Ulitsa Angel Kanchev 25, Ruse, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Ruse Drainage Network" },
                    { 6, "Ulitsa Gotse Delchev 12, Blagoevgrad, Bulgaria", true, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Blagoevgrad Water Storage Facility" },
                    { 7, "Ulitsa Hristo Botev 15, Pleven, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Pleven Irrigation System" },
                    { 8, "Ulitsa General Gurko 19, Stara Zagora, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Stara Zagora Drainage Improvement" },
                    { 9, "Ulitsa Vasil Levski 22, Dobrich, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Dobrich Hydrophore Installation" },
                    { 10, "Ulitsa Osvobozhdenie 8, Haskovo, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Haskovo Water Distribution Network" },
                    { 11, "Ulitsa Simeon Veliki 18, Shumen, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Shumen Pump Station" },
                    { 12, "Ulitsa Neofit Rilski 5, Kyustendil, Bulgaria", false, null, "https://www.handyman.net.au/wp-content/uploads/old_images/g-lawns-lawnirrigation-DIAGRAM.jpg", "Kyustendil Water Filtration Upgrade" }
                });

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

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "HoursWorked", "IsCompleted", "ProfProjectId", "Title" },
                values: new object[,]
                {
                    { 1, "Perform an initial inspection of the site for water filtration system installation.", 10, true, 1, "Inspect site" },
                    { 2, "Ensure all filtration components are available and functional.", 8, true, 1, "Prepare filtration materials" },
                    { 3, "Set up and install the filtration unit according to the design.", 15, true, 1, "Install filtration unit" },
                    { 4, "Run tests to ensure water quality meets standards.", 12, true, 1, "Test water quality" },
                    { 5, "Document the project and hand over the report to the client.", 5, true, 1, "Prepare final report" },
                    { 6, "Survey the area to plan the layout for irrigation pipes.", 8, true, 2, "Survey irrigation layout" },
                    { 7, "Install the main pipeline for water distribution.", 20, true, 2, "Lay main pipeline" },
                    { 8, "Install and configure the drip irrigation system.", 18, true, 2, "Set up drip irrigation" },
                    { 9, "Test the water flow through the irrigation system.", 10, true, 2, "Test irrigation flow" },
                    { 10, "Train the client on using and maintaining the irrigation system.", 5, true, 2, "Client training" },
                    { 11, "Survey the installation site for hydrophore system.", 6, true, 3, "Site survey for installation" },
                    { 12, "Install the main water tank for the system.", 14, true, 3, "Install water tank" },
                    { 13, "Set up the hydrophore pump for water pressurization.", 20, true, 3, "Install hydrophore pump" },
                    { 14, "Test the pressure levels of the hydrophore system.", 8, true, 3, "Test system pressure" },
                    { 15, "Perform a final check to ensure the system is running smoothly.", 6, true, 3, "Final system check" },
                    { 16, "Survey the land to place the water storage tank.", 5, true, 4, "Survey location for tank" },
                    { 17, "Set up and install the water storage tank.", 15, true, 4, "Install water tank" },
                    { 18, "Set up the pump for the water storage system.", 12, true, 4, "Install pump system" },
                    { 19, "Test the system to ensure it can hold the required volume.", 10, true, 4, "Test system capacity" },
                    { 20, "Inspect the entire water storage system for any issues.", 5, true, 4, "Final inspection" },
                    { 21, "Survey the layout for the water distribution system.", 7, true, 5, "Survey for pipe layout" },
                    { 22, "Install the main water line for distribution.", 18, true, 5, "Install main water line" },
                    { 23, "Install and configure water meters for distribution monitoring.", 10, true, 5, "Set up water meters" },
                    { 24, "Test the water flow and pressure across the system.", 8, true, 5, "Test water flow" },
                    { 25, "Hand over the completed system to the client.", 5, true, 5, "Client handover" },
                    { 26, "Survey the site for drainage system setup.", 6, true, 6, "Inspect drainage area" },
                    { 27, "Prepare the ground for drainage pipes.", 12, true, 6, "Dig trenches for pipes" },
                    { 28, "Place and secure the drainage pipes.", 20, true, 6, "Install drainage pipes" },
                    { 29, "Cover the installed pipes with soil.", 10, true, 6, "Backfill trenches" },
                    { 30, "Run water through the system to ensure functionality.", 8, true, 6, "Test drainage system" },
                    { 31, "Survey the site for pump installation.", 6, true, 7, "Inspect pump installation site" },
                    { 32, "Set up the pump unit for the irrigation system.", 8, false, 7, "Install pump unit" },
                    { 33, "Ensure that the pump operates at the correct pressure.", 6, false, 7, "Test pump functionality" },
                    { 34, "Connect the pump to the main irrigation pipes.", 10, false, 7, "Connect pipes" },
                    { 35, "Verify the entire pump installation system.", 5, false, 7, "System check" },
                    { 36, "Survey the area for installation of the water supply system.", 8, false, 8, "Survey the water supply area" },
                    { 37, "Install the primary pipes for the water supply system.", 20, false, 8, "Lay pipes for water supply" },
                    { 38, "Link the supply line to the water filtration system.", 12, false, 8, "Connect supply to filtration" },
                    { 39, "Install water meters for monitoring.", 8, false, 8, "Install water meters" },
                    { 40, "Ensure the water pressure meets standards.", 10, false, 8, "Test water pressure" },
                    { 41, "Inspect the site for desalination equipment.", 7, true, 9, "Survey desalination site" },
                    { 42, "Set up the desalination unit to process seawater.", 15, false, 9, "Install desalination unit" },
                    { 43, "Link the desalination unit to the main water supply.", 12, false, 9, "Connect desalination to water supply" },
                    { 44, "Check water quality after desalination process.", 10, false, 9, "Test water quality post-desalination" },
                    { 45, "Run the desalination system in full operation for a trial period.", 8, false, 9, "System commissioning" },
                    { 46, "Survey the site for installation of the treatment plant.", 6, false, 10, "Survey treatment plant site" },
                    { 47, "Set up the primary water treatment system.", 20, false, 10, "Install treatment system" },
                    { 48, "Test the operation of the treatment system.", 12, false, 10, "Test system operation" },
                    { 49, "Test the effectiveness of the filtration system.", 8, false, 10, "Check filtration quality" },
                    { 50, "Complete the installation and setup of the treatment plant.", 10, false, 10, "Finalize system setup" },
                    { 51, "Inspect the site for rainwater harvesting system setup.", 8, false, 11, "Survey site for harvesting system" },
                    { 52, "Set up the rainwater collection and filtration system.", 18, false, 11, "Install harvesting system" },
                    { 53, "Link the system to the main water storage tank.", 12, false, 11, "Connect to storage tank" },
                    { 54, "Test the entire system for proper operation.", 10, false, 11, "Test system functionality" },
                    { 55, "Train the client on operating and maintaining the system.", 6, false, 11, "Client handover" },
                    { 56, "Survey the industrial site for water supply needs.", 8, false, 12, "Survey industrial site" },
                    { 57, "Install pipes and pumps for the industrial water system.", 20, false, 12, "Install water supply system" },
                    { 58, "Test the system for water pressure and flow.", 15, false, 12, "Test water flow" },
                    { 59, "Link the water supply system to the filtration unit.", 10, false, 12, "Connect to filtration" },
                    { 60, "Perform a final check on the system functionality.", 7, false, 12, "System verification" }
                });

            migrationBuilder.InsertData(
                table: "TasksMaterials",
                columns: new[] { "MaterialId", "TaskId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 4, 2 },
                    { 5, 2 },
                    { 6, 3 },
                    { 7, 3 },
                    { 8, 4 },
                    { 9, 4 },
                    { 10, 5 },
                    { 11, 5 },
                    { 12, 6 },
                    { 13, 6 },
                    { 14, 7 },
                    { 15, 7 },
                    { 16, 8 },
                    { 17, 8 },
                    { 18, 9 },
                    { 19, 9 },
                    { 20, 10 },
                    { 21, 10 },
                    { 22, 11 },
                    { 23, 11 },
                    { 24, 12 },
                    { 24, 17 },
                    { 24, 47 },
                    { 24, 52 },
                    { 25, 12 },
                    { 25, 17 },
                    { 25, 47 },
                    { 25, 52 },
                    { 26, 13 },
                    { 26, 18 },
                    { 27, 13 },
                    { 27, 18 },
                    { 28, 14 },
                    { 28, 19 },
                    { 29, 14 },
                    { 30, 15 },
                    { 31, 15 },
                    { 32, 16 },
                    { 33, 16 },
                    { 34, 19 },
                    { 35, 20 },
                    { 36, 20 },
                    { 37, 21 },
                    { 37, 31 },
                    { 38, 21 },
                    { 38, 31 },
                    { 39, 22 },
                    { 39, 32 },
                    { 39, 37 },
                    { 40, 22 },
                    { 40, 32 },
                    { 40, 37 },
                    { 41, 23 },
                    { 41, 33 },
                    { 41, 38 },
                    { 42, 23 },
                    { 42, 33 },
                    { 42, 38 },
                    { 42, 58 },
                    { 43, 24 },
                    { 43, 34 },
                    { 43, 39 },
                    { 44, 24 },
                    { 44, 34 },
                    { 44, 39 },
                    { 45, 25 },
                    { 45, 35 },
                    { 45, 40 },
                    { 46, 25 },
                    { 46, 35 },
                    { 46, 40 },
                    { 47, 26 },
                    { 47, 36 },
                    { 47, 41 },
                    { 47, 46 },
                    { 47, 51 },
                    { 47, 56 },
                    { 48, 26 },
                    { 48, 36 },
                    { 48, 41 },
                    { 48, 46 },
                    { 48, 51 },
                    { 48, 56 },
                    { 49, 27 },
                    { 49, 42 },
                    { 49, 48 },
                    { 49, 53 },
                    { 49, 57 },
                    { 50, 27 },
                    { 50, 42 },
                    { 50, 48 },
                    { 50, 53 },
                    { 50, 57 },
                    { 51, 28 },
                    { 51, 43 },
                    { 51, 49 },
                    { 51, 54 },
                    { 51, 59 },
                    { 52, 28 },
                    { 52, 43 },
                    { 52, 49 },
                    { 52, 54 },
                    { 52, 58 },
                    { 52, 59 },
                    { 53, 29 },
                    { 53, 44 },
                    { 53, 50 },
                    { 53, 55 },
                    { 53, 60 },
                    { 54, 29 },
                    { 54, 44 },
                    { 54, 50 },
                    { 54, 55 },
                    { 54, 60 },
                    { 55, 30 },
                    { 55, 45 },
                    { 56, 30 },
                    { 56, 45 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectsRequests_ProfProjectRequestId",
                table: "Projects",
                column: "ProfProjectRequestId",
                principalTable: "ProjectsRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectsRequests_ProfProjectRequestId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "SpecialistRequests");

            migrationBuilder.DropTable(
                name: "WorkerRequests");

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Penalties",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 9, 4 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 10, 5 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 11, 5 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 12, 6 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 14, 7 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 15, 7 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 16, 8 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 17, 8 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 18, 9 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 19, 9 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 20, 10 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 21, 10 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 22, 11 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 23, 11 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 24, 12 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 24, 17 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 24, 47 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 24, 52 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 25, 12 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 25, 17 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 25, 47 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 25, 52 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 26, 13 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 26, 18 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 27, 13 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 27, 18 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 28, 14 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 28, 19 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 29, 14 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 30, 15 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 31, 15 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 32, 16 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 33, 16 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 34, 19 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 35, 20 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 36, 20 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 37, 21 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 37, 31 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 38, 21 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 38, 31 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 39, 22 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 39, 32 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 39, 37 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 40, 22 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 40, 32 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 40, 37 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 41, 23 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 41, 33 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 41, 38 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 42, 23 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 42, 33 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 42, 38 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 42, 58 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 43, 24 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 43, 34 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 43, 39 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 44, 24 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 44, 34 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 44, 39 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 45, 25 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 45, 35 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 45, 40 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 46, 25 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 46, 35 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 46, 40 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 26 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 36 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 41 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 46 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 51 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 47, 56 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 26 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 36 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 41 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 46 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 51 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 48, 56 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 49, 27 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 49, 42 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 49, 48 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 49, 53 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 49, 57 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 50, 27 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 50, 42 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 50, 48 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 50, 53 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 50, 57 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 51, 28 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 51, 43 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 51, 49 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 51, 54 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 51, 59 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 28 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 43 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 49 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 54 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 58 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 52, 59 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 53, 29 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 53, 44 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 53, 50 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 53, 55 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 53, 60 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 54, 29 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 54, 44 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 54, 50 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 54, 55 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 54, 60 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 55, 30 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 55, 45 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 56, 30 });

            migrationBuilder.DeleteData(
                table: "TasksMaterials",
                keyColumns: new[] { "MaterialId", "TaskId" },
                keyValues: new object[] { 56, 45 });

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Materials",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tasks",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(65)",
                oldMaxLength: 65);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Projects",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<int>(
                name: "ProfProjectRequestId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectsRequests_ProfProjectRequestId",
                table: "Projects",
                column: "ProfProjectRequestId",
                principalTable: "ProjectsRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
