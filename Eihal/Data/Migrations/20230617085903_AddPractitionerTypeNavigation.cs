using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddPractitionerTypeNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PractitionerTypeId",
                table: "UserProfiles",
                column: "PractitionerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_PractitionerTypes_PractitionerTypeId",
                table: "UserProfiles",
                column: "PractitionerTypeId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_PractitionerTypes_PractitionerTypeId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_PractitionerTypeId",
                table: "UserProfiles");
        }
    }
}
