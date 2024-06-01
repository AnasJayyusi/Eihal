using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class RenameColumnInGenralSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsProfessionalCategory",
                table: "GeneralSettings",
                newName: "IsProfessionalCategoryRequired");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsProfessionalCategoryRequired",
                table: "GeneralSettings",
                newName: "IsProfessionalCategory");
        }
    }
}
