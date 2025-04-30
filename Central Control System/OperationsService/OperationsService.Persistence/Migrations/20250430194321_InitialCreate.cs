using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OperationsService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EventTypeGID = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CoordinatorGID = table.Column<Guid>(type: "uuid", nullable: false),
                    DispatcherGID = table.Column<Guid>(type: "uuid", nullable: false),
                    EventStatusGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "EventStatuses",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatuses", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "OperationTaskStatuses",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTaskStatuses", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "OperationWorkers",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    IdentificationCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationWorkers", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EventGID = table.Column<Guid>(type: "uuid", nullable: false),
                    LeaderGID = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GID);
                    table.ForeignKey(
                        name: "FK_Groups_Events_EventGID",
                        column: x => x.EventGID,
                        principalTable: "Events",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourcesEvents",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceGID = table.Column<Guid>(type: "uuid", nullable: false),
                    EventGID = table.Column<Guid>(type: "uuid", nullable: false),
                    MeasurementUnitGID = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredQuantity = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    AvailableQuantity = table.Column<decimal>(type: "numeric(18,8)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourcesEvents", x => x.GID);
                    table.ForeignKey(
                        name: "FK_ResourcesEvents_Events_EventGID",
                        column: x => x.EventGID,
                        principalTable: "Events",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<Guid>(type: "uuid", nullable: false),
                    To = table.Column<Guid>(type: "uuid", nullable: false),
                    EventGID = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.GID);
                    table.ForeignKey(
                        name: "FK_Messages_Events_EventGID",
                        column: x => x.EventGID,
                        principalTable: "Events",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_OperationWorkers_From",
                        column: x => x.From,
                        principalTable: "OperationWorkers",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_OperationWorkers_To",
                        column: x => x.To,
                        principalTable: "OperationWorkers",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTasks",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    GroupGID = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskStatusGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTasks", x => x.GID);
                    table.ForeignKey(
                        name: "FK_OperationTasks_Groups_GroupGID",
                        column: x => x.GroupGID,
                        principalTable: "Groups",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationTasks_OperationTaskStatuses_TaskStatusGID",
                        column: x => x.TaskStatusGID,
                        principalTable: "OperationTaskStatuses",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_EventGID",
                table: "Groups",
                column: "EventGID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_EventGID",
                table: "Messages",
                column: "EventGID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_From",
                table: "Messages",
                column: "From");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_To",
                table: "Messages",
                column: "To");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTasks_GroupGID",
                table: "OperationTasks",
                column: "GroupGID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTasks_TaskStatusGID",
                table: "OperationTasks",
                column: "TaskStatusGID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourcesEvents_EventGID",
                table: "ResourcesEvents",
                column: "EventGID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStatuses");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "OperationTasks");

            migrationBuilder.DropTable(
                name: "ResourcesEvents");

            migrationBuilder.DropTable(
                name: "OperationWorkers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "OperationTaskStatuses");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
