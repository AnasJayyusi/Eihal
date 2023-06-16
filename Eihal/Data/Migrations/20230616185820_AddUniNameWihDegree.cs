using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddUniNameWihDegree : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniversityNameAr",
                table: "Certifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityNameEn",
                table: "Certifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityNameAr",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "UniversityNameEn",
                table: "Certifications");
        }
    }
}
