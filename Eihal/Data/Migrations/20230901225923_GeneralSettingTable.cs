using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class GeneralSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicalSpecialityId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VatValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SitePercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services",
                column: "ClinicalSpecialityId",
                principalTable: "ClinicalSpecialities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "GeneralSettings");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicalSpecialityId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ClinicalSpecialities_ClinicalSpecialityId",
                table: "Services",
                column: "ClinicalSpecialityId",
                principalTable: "ClinicalSpecialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
