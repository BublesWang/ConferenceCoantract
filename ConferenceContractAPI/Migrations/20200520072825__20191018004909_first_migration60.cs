using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration60 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MapId",
                table: "InviteCodeRecord",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "InviteCodeRecord",
                newName: "MapId");
        }
    }
}
