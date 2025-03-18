using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    MobilePhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.GID);
                });

            migrationBuilder.CreateTable(
                name: "VolunteersDistricts",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    VolunteerGID = table.Column<Guid>(type: "uuid", nullable: false),
                    DistrictGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteersDistricts", x => x.GID);
                    table.ForeignKey(
                        name: "FK_VolunteersDistricts_Volunteers_VolunteerGID",
                        column: x => x.VolunteerGID,
                        principalTable: "Volunteers",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VolunteersGroups",
                columns: table => new
                {
                    GID = table.Column<Guid>(type: "uuid", nullable: false),
                    VolunteerGID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupGID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteersGroups", x => x.GID);
                    table.ForeignKey(
                        name: "FK_VolunteersGroups_Volunteers_VolunteerGID",
                        column: x => x.VolunteerGID,
                        principalTable: "Volunteers",
                        principalColumn: "GID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VolunteersDistricts_VolunteerGID_DistrictGID",
                table: "VolunteersDistricts",
                columns: new[] { "VolunteerGID", "DistrictGID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VolunteersGroups_VolunteerGID_GroupGID",
                table: "VolunteersGroups",
                columns: new[] { "VolunteerGID", "GroupGID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VolunteersDistricts");

            migrationBuilder.DropTable(
                name: "VolunteersGroups");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
