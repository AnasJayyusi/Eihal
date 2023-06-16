using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddDegreesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DegreeId",
                table: "Attachments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniqueFileName",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Degrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degrees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_DegreeId",
                table: "Attachments",
                column: "DegreeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Degrees_DegreeId",
                table: "Attachments",
                column: "DegreeId",
                principalTable: "Degrees",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Degrees_DegreeId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "Degrees");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_DegreeId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "DegreeId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "UniqueFileName",
                table: "Attachments");
        }
    }
}
