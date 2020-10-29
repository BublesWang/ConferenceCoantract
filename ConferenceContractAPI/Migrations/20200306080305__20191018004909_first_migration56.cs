using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration56 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyServicePackName",
                table: "ConferenceOnsite",
                maxLength: 1500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ConferenceOnsite",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyServicePackName",
                table: "ConferenceOnsite");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ConferenceOnsite");
        }
    }
}
