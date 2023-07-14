using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddTimeClinicLocationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeClinicLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(type: "int", nullable: false),
                    SundayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SundayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SundayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    MondayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    TuesdayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuesdayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuesdayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WednesdayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WednesdayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThursdayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThursdayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    FridayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FridayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FridayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayOpenAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayClosedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayIsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClinicLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeClinicLocations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClinicLocations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClinicLocations_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClinicLocations_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeClinicLocations_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_CityId",
                table: "TimeClinicLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_CountryId",
                table: "TimeClinicLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_DistrictId",
                table: "TimeClinicLocations",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_StateId",
                table: "TimeClinicLocations",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_UserProfileId",
                table: "TimeClinicLocations",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeClinicLocations");
        }
    }
}
