using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UtilsService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementUnits",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnits", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "ResourceMeasurementUnits",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    UnitGID = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceGID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceMeasurementUnits", x => x.GID);
                    table.ForeignKey(
                        name: "FK_ResourceMeasurementUnits_MeasurementUnits_UnitGID",
                        column: x => x.UnitGID,
                        principalTable: "MeasurementUnits",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResourceMeasurementUnits_Resources_ResourceGID",
                        column: x => x.ResourceGID,
                        principalTable: "Resources",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMeasurementUnits_ResourceGID",
                table: "ResourceMeasurementUnits",
                column: "ResourceGID");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceMeasurementUnits_UnitGID",
                table: "ResourceMeasurementUnits",
                column: "UnitGID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "ResourceMeasurementUnits");

            migrationBuilder.DropTable(
                name: "MeasurementUnits");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
