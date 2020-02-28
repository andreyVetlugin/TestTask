using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class egissoV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChildIsReason",
                table: "KpCodes");

            migrationBuilder.AlterColumn<string>(
                name: "OkeiCode",
                table: "MeasureUnits",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OkeiCode",
                table: "MeasureUnits",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsChildIsReason",
                table: "KpCodes",
                nullable: false,
                defaultValue: false);
        }
    }
}
