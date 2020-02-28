using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class ChangePensionCaseNumberTypeIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PensionCaseNumber",
                table: "PersonInfos",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PensionCaseNumber",
                table: "PersonInfos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
