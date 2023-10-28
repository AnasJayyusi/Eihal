using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddServicesLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceLevelId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServiceLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceLevelId",
                table: "Services",
                column: "ServiceLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceLevels_ServiceLevelId",
                table: "Services",
                column: "ServiceLevelId",
                principalTable: "ServiceLevels",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceLevels_ServiceLevelId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "ServiceLevels");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceLevelId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceLevelId",
                table: "Services");
        }
    }
}
