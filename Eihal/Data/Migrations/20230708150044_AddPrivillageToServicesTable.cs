using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddPrivillageToServicesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrivillageId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubPrivillageId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_PrivillageId",
                table: "Services",
                column: "PrivillageId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_SubPrivillageId",
                table: "Services",
                column: "SubPrivillageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Privillages_PrivillageId",
                table: "Services",
                column: "PrivillageId",
                principalTable: "Privillages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_SubPrivillages_SubPrivillageId",
                table: "Services",
                column: "SubPrivillageId",
                principalTable: "SubPrivillages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Privillages_PrivillageId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_SubPrivillages_SubPrivillageId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_PrivillageId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_SubPrivillageId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "PrivillageId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "SubPrivillageId",
                table: "Services");
        }
    }
}
