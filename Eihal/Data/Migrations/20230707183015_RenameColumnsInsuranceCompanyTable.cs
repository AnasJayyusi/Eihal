using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class RenameColumnsInsuranceCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "InsuranceCompanies",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "InsuranceCompanies",
                newName: "TitleAr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "InsuranceCompanies",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "InsuranceCompanies",
                newName: "NameAr");
        }
    }
}
