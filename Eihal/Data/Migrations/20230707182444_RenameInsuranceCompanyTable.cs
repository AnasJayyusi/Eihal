using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class RenameInsuranceCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InsuranceCompanines",
                table: "InsuranceCompanines");

            migrationBuilder.RenameTable(
                name: "InsuranceCompanines",
                newName: "InsuranceCompanies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InsuranceCompanies",
                table: "InsuranceCompanies",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InsuranceCompanies",
                table: "InsuranceCompanies");

            migrationBuilder.RenameTable(
                name: "InsuranceCompanies",
                newName: "InsuranceCompanines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InsuranceCompanines",
                table: "InsuranceCompanines",
                column: "Id");
        }
    }
}
