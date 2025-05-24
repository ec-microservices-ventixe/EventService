using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEventPackages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleSlots_EventSchedules_ScheduleId",
                table: "ScheduleSlots");

            migrationBuilder.DropTable(
                name: "EventSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "ScheduleSlots",
                newName: "EventId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlots_ScheduleId",
                table: "ScheduleSlots",
                newName: "IX_ScheduleSlots_EventId");

            migrationBuilder.CreateTable(
                name: "EventPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsSeating = table.Column<bool>(type: "bit", nullable: true),
                    Benefits = table.Column<string>(type: "varchar(120)", nullable: false),
                    ExtraFeeInProcent = table.Column<float>(type: "real", nullable: false),
                    NumberOfTickets = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventPackages_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventPackages_EventId",
                table: "EventPackages",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleSlots_Events_EventId",
                table: "ScheduleSlots",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleSlots_Events_EventId",
                table: "ScheduleSlots");

            migrationBuilder.DropTable(
                name: "EventPackages");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "ScheduleSlots",
                newName: "ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_ScheduleSlots_EventId",
                table: "ScheduleSlots",
                newName: "IX_ScheduleSlots_ScheduleId");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSchedules_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSchedules_EventId",
                table: "EventSchedules",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleSlots_EventSchedules_ScheduleId",
                table: "ScheduleSlots",
                column: "ScheduleId",
                principalTable: "EventSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
