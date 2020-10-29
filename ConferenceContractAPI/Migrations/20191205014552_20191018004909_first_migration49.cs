﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFianceRecord",
                table: "PersonContract",
                nullable: true,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFianceRecord",
                table: "PersonContract");
        }
    }
}
