using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RemarkCode",
                table: "CompanyServicePack",
                maxLength: 500,
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RemarkDic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContentCn = table.Column<string>(type: "text", nullable: true),
                    ContentEn = table.Column<string>(type: "text", nullable: true),
                    ContentJp = table.Column<string>(type: "text", nullable: true),
                    ContentCode = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemarkDic", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemarkDic_ContentCode",
                table: "RemarkDic",
                column: "ContentCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemarkDic");

            migrationBuilder.DropColumn(
                name: "RemarkCode",
                table: "CompanyServicePack");
        }
    }
}
