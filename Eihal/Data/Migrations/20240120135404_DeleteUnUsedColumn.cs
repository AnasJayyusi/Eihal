using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class DeleteUnUsedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueFileName",
                table: "RequiredAttachments");

            migrationBuilder.DropColumn(
                name: "UniqueFileName",
                table: "Certifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UniqueFileName",
                table: "RequiredAttachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UniqueFileName",
                table: "Certifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
