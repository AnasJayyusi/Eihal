using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class EditAttachmentTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Degrees_DegreeId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_UserProfiles_UserProfileId",
                table: "Attachments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Certifications");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_UserProfileId",
                table: "Certifications",
                newName: "IX_Certifications_UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Attachments_DegreeId",
                table: "Certifications",
                newName: "IX_Certifications_DegreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certifications",
                table: "Certifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_Degrees_DegreeId",
                table: "Certifications",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_UserProfiles_UserProfileId",
                table: "Certifications",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_Degrees_DegreeId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_UserProfiles_UserProfileId",
                table: "Certifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certifications",
                table: "Certifications");

            migrationBuilder.RenameTable(
                name: "Certifications",
                newName: "Attachments");

            migrationBuilder.RenameIndex(
                name: "IX_Certifications_UserProfileId",
                table: "Attachments",
                newName: "IX_Attachments_UserProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Certifications_DegreeId",
                table: "Attachments",
                newName: "IX_Attachments_DegreeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Degrees_DegreeId",
                table: "Attachments",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_UserProfiles_UserProfileId",
                table: "Attachments",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id");
        }
    }
}
