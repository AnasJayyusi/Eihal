using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class UpdateReferralRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralRequests_UserProfiles_UserId",
                table: "ReferralRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReferralRequests_UserId",
                table: "ReferralRequests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ReferralRequests",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "ReferralRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "ReferralRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRequests_AssignedToUserId",
                table: "ReferralRequests",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRequests_CreatedByUserId",
                table: "ReferralRequests",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralRequests_UserProfiles_AssignedToUserId",
                table: "ReferralRequests",
                column: "AssignedToUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralRequests_UserProfiles_CreatedByUserId",
                table: "ReferralRequests",
                column: "CreatedByUserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReferralRequests_UserProfiles_AssignedToUserId",
                table: "ReferralRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferralRequests_UserProfiles_CreatedByUserId",
                table: "ReferralRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReferralRequests_AssignedToUserId",
                table: "ReferralRequests");

            migrationBuilder.DropIndex(
                name: "IX_ReferralRequests_CreatedByUserId",
                table: "ReferralRequests");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "ReferralRequests");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ReferralRequests");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ReferralRequests",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferralRequests_UserId",
                table: "ReferralRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReferralRequests_UserProfiles_UserId",
                table: "ReferralRequests",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
