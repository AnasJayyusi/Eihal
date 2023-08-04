using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class SupoortMultiServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralRequests_Services_ServiceId",
                table: "ReferralRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_OrderDetails_OrderDetailId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_OrderDetailId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_ReferralRequests_ServiceId",
                table: "ReferralRequests");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ReferralRequests");

            migrationBuilder.CreateTable(
                name: "OrderDetailServices",
                columns: table => new
                {
                    OrderDetailsId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetailServices", x => new { x.OrderDetailsId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_OrderDetailServices_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetailServices_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetailServices_ServicesId",
                table: "OrderDetailServices",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetailServices");

            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "ReferralRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Services_OrderDetailId",
                table: "Services",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRequests_ServiceId",
                table: "ReferralRequests",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralRequests_Services_ServiceId",
                table: "ReferralRequests",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OrderDetails_OrderDetailId",
                table: "Services",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
