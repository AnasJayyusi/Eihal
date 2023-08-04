using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeClinicLocations_UserProfileId",
                table: "TimeClinicLocations");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ReferralRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    ChronicDisease = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDetails_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_UserProfileId",
                table: "TimeClinicLocations",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrderDetailId",
                table: "Services",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRequests_OrderId",
                table: "ReferralRequests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CityId",
                table: "OrderDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_CountryId",
                table: "OrderDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_StateId",
                table: "OrderDetails",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralRequests_OrderDetails_OrderId",
                table: "ReferralRequests",
                column: "OrderId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OrderDetails_OrderDetailId",
                table: "Services",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralRequests_OrderDetails_OrderId",
                table: "ReferralRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_OrderDetails_OrderDetailId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_TimeClinicLocations_UserProfileId",
                table: "TimeClinicLocations");

            migrationBuilder.DropIndex(
                name: "IX_Services_OrderDetailId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_ReferralRequests_OrderId",
                table: "ReferralRequests");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ReferralRequests");

            migrationBuilder.CreateIndex(
                name: "IX_TimeClinicLocations_UserProfileId",
                table: "TimeClinicLocations",
                column: "UserProfileId");
        }
    }
}
