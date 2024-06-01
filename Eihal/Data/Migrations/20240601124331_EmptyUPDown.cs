using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eihal.Data.Migrations
{
    public partial class EmptyUPDown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
            name: "IsCertifactionsAttachmnetsRequired",
            table: "GeneralSettings",
            type: "bit",
            nullable: false,
            defaultValue: false);
          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                  name: "IsCertifactionsAttachmnetsRequired",
                  table: "GeneralSettings");
        }
    }
}
