using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class AddRelationsToUserProfileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "UserProfiles",
                newName: "StateId");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "UserProfiles",
                newName: "ProfessionalRankId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "UserProfiles",
                newName: "CityId");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtiesIds",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtiesTitlesAr",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialtiesTitlesEn",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CityId",
                table: "UserProfiles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProfessionalRankId",
                table: "UserProfiles",
                column: "ProfessionalRankId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_StateId",
                table: "UserProfiles",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Cities_CityId",
                table: "UserProfiles",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Countries_CountryId",
                table: "UserProfiles",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_ProfessionalRanks_ProfessionalRankId",
                table: "UserProfiles",
                column: "ProfessionalRankId",
                principalTable: "ProfessionalRanks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_States_StateId",
                table: "UserProfiles",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Cities_CityId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Countries_CountryId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_ProfessionalRanks_ProfessionalRankId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_States_StateId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CityId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ProfessionalRankId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_StateId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SpecialtiesIds",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SpecialtiesTitlesAr",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "SpecialtiesTitlesEn",
                table: "UserProfiles");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "UserProfiles",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ProfessionalRankId",
                table: "UserProfiles",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "UserProfiles",
                newName: "CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Speciality",
                table: "UserProfiles",
                type: "nvarchar(max)",
                maxLength: 4096,
                nullable: true);
        }
    }
}
