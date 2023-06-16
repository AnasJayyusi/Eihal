using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddProfileStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "UserProfiles");

            migrationBuilder.AddColumn<int>(
                name: "ProfileStatus",
                table: "UserProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileStatus",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "UserProfiles",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true);
        }
    }
}
