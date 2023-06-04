using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddColumnsToAspNetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PractitionerTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessionalRankId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalRank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalRank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PractitionerTypeId",
                table: "AspNetUsers",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfessionalRankId",
                table: "AspNetUsers",
                column: "ProfessionalRankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId",
                principalTable: "AccountTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PractitionerTypes_PractitionerTypeId",
                table: "AspNetUsers",
                column: "PractitionerTypeId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfessionalRank_ProfessionalRankId",
                table: "AspNetUsers",
                column: "ProfessionalRankId",
                principalTable: "ProfessionalRank",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountTypes_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PractitionerTypes_PractitionerTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfessionalRank_ProfessionalRankId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AccountTypes");

            migrationBuilder.DropTable(
                name: "PractitionerTypes");

            migrationBuilder.DropTable(
                name: "ProfessionalRank");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PractitionerTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfessionalRankId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PractitionerTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfessionalRankId",
                table: "AspNetUsers");
        }
    }
}
