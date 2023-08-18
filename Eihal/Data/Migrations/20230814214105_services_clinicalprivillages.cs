using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class services_clinicalprivillages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Privillages_PrivillageId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PrivillageId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PrivillageId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "ClinicalSpecialityId",
                table: "Services",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Services_ClinicalSpecialityId",
                table: "Services",
                column: "ClinicalSpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services",
                column: "ClinicalSpecialityId",
                principalTable: "ClinicalSpecialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_ClinicalSpecialityId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ClinicalSpecialityId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "PrivillageId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_PrivillageId",
                table: "Services",
                column: "PrivillageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Privillages_PrivillageId",
                table: "Services",
                column: "PrivillageId",
                principalTable: "Privillages",
                principalColumn: "Id");
        }
    }
}
