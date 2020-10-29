using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration54 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiviType",
                table: "ConferenceOnsite");

            migrationBuilder.AddColumn<string>(
                name: "CompanyServicePackId",
                table: "ConferenceOnsite",
                maxLength: 1500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyServicePackId",
                table: "ConferenceOnsite");

            migrationBuilder.AddColumn<string>(
                name: "ActiviType",
                table: "ConferenceOnsite",
                maxLength: 150,
                nullable: true);
        }
    }
}
