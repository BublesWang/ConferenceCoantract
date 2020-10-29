using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration59 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InviteCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InviteCodeNumber = table.Column<string>(maxLength: 500, nullable: true),
                    CompanyServicePackId = table.Column<string>(maxLength: 500, nullable: true),
                    Count = table.Column<int>(nullable: false),
                    Year = table.Column<string>(maxLength: 50, nullable: true),
                    WebSite = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InviteCodeRecord",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    InviteCodeId = table.Column<Guid>(nullable: true),
                    MemberPK = table.Column<string>(maxLength: 500, nullable: true),
                    MemberName = table.Column<string>(maxLength: 500, nullable: true),
                    UseDate = table.Column<string>(maxLength: 500, nullable: true),
                    PersonContractId = table.Column<string>(maxLength: 500, nullable: true),
                    PersonContractNumber = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteCodeRecord", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_InviteCodeRecord_InviteCode_InviteCodeId",
                        column: x => x.InviteCodeId,
                        principalTable: "InviteCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InviteCodeRecord_InviteCodeId",
                table: "InviteCodeRecord",
                column: "InviteCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InviteCodeRecord");

            migrationBuilder.DropTable(
                name: "InviteCode");
        }
    }
}
