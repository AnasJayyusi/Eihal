using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class EditPRTableAndAddSpecialtyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfessionalRank_ProfessionalRankId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfessionalRank",
                table: "ProfessionalRank");

            migrationBuilder.RenameTable(
                name: "ProfessionalRank",
                newName: "ProfessionalRanks");

            migrationBuilder.AddColumn<int>(
                name: "PractitionerTypeId",
                table: "ProfessionalRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfessionalRanks",
                table: "ProfessionalRanks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PractitionerTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialties_PractitionerTypes_PractitionerTypeId",
                        column: x => x.PractitionerTypeId,
                        principalTable: "PractitionerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalRanks_PractitionerTypeId",
                table: "ProfessionalRanks",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_PractitionerTypeId",
                table: "Specialties",
                column: "PractitionerTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfessionalRanks_ProfessionalRankId",
                table: "AspNetUsers",
                column: "ProfessionalRankId",
                principalTable: "ProfessionalRanks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionalRanks_PractitionerTypes_PractitionerTypeId",
                table: "ProfessionalRanks",
                column: "PractitionerTypeId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfessionalRanks_ProfessionalRankId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionalRanks_PractitionerTypes_PractitionerTypeId",
                table: "ProfessionalRanks");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfessionalRanks",
                table: "ProfessionalRanks");

            migrationBuilder.DropIndex(
                name: "IX_ProfessionalRanks_PractitionerTypeId",
                table: "ProfessionalRanks");

            migrationBuilder.DropColumn(
                name: "PractitionerTypeId",
                table: "ProfessionalRanks");

            migrationBuilder.RenameTable(
                name: "ProfessionalRanks",
                newName: "ProfessionalRank");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfessionalRank",
                table: "ProfessionalRank",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfessionalRank_ProfessionalRankId",
                table: "AspNetUsers",
                column: "ProfessionalRankId",
                principalTable: "ProfessionalRank",
                principalColumn: "Id");
        }
    }
}
