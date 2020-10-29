using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration64 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperateRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContractNumber = table.Column<string>(maxLength: 500, nullable: true),
                    OperateTime = table.Column<string>(maxLength: 50, nullable: true),
                    Operator = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperateRecord", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperateRecord");
        }
    }
}
