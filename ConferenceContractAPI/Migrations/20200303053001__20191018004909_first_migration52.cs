using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration52 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConferenceOnsite",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ContractNumber = table.Column<string>(maxLength: 150, nullable: true),
                    CompanyName = table.Column<string>(maxLength: 150, nullable: true),
                    ActiviType = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(maxLength: 150, nullable: true),
                    PayType = table.Column<string>(maxLength: 150, nullable: true),
                    Credited = table.Column<string>(maxLength: 150, nullable: true),
                    AddDate = table.Column<string>(maxLength: 150, nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 50, nullable: true),
                    ModefieldOn = table.Column<DateTime>(nullable: true),
                    ModefieldBy = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceOnsite", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceOnsite");
        }
    }
}
