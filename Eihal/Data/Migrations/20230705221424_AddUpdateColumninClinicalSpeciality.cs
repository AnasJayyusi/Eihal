using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddUpdateColumninClinicalSpeciality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalSpecialities_PractitionerTypes_SpecialityId",
                table: "ClinicalSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_ClinicalSpecialities_SpecialityId",
                table: "ClinicalSpecialities");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "ClinicalSpecialities");

            migrationBuilder.AddColumn<int>(
                name: "PractitionerTypeId",
                table: "ClinicalSpecialities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalSpecialities_PractitionerTypeId",
                table: "ClinicalSpecialities",
                column: "PractitionerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalSpecialities_PractitionerTypes_PractitionerTypeId",
                table: "ClinicalSpecialities",
                column: "PractitionerTypeId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicalSpecialities_PractitionerTypes_PractitionerTypeId",
                table: "ClinicalSpecialities");

            migrationBuilder.DropIndex(
                name: "IX_ClinicalSpecialities_PractitionerTypeId",
                table: "ClinicalSpecialities");

            migrationBuilder.DropColumn(
                name: "PractitionerTypeId",
                table: "ClinicalSpecialities");

            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "ClinicalSpecialities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicalSpecialities_SpecialityId",
                table: "ClinicalSpecialities",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicalSpecialities_PractitionerTypes_SpecialityId",
                table: "ClinicalSpecialities",
                column: "SpecialityId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id");
        }
    }
}
