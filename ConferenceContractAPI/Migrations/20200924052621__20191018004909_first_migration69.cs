using Microsoft.EntityFrameworkCore.Migrations;

namespace ConferenceContractAPI.Migrations
{
    public partial class _20191018004909_first_migration69 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaidAmount",
                table: "PersonContract",
                maxLength: 150,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "DelegateServicePackDiscount",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true,
                oldDefaultValue: "2019");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "PersonContract");

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "DelegateServicePackDiscount",
                maxLength: 50,
                nullable: true,
                defaultValue: "2019",
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
