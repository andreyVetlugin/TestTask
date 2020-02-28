using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AlterExtraPayVariantFieldsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VyslugaMultiplier",
                table: "ExtraPayVariants",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "VyslugaDivPerc",
                table: "ExtraPayVariants",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "UralMultiplier",
                table: "ExtraPayVariants",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PremiumPerc",
                table: "ExtraPayVariants",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "MatSupportMultiplier",
                table: "ExtraPayVariants",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "VyslugaMultiplier",
                table: "ExtraPayVariants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "VyslugaDivPerc",
                table: "ExtraPayVariants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UralMultiplier",
                table: "ExtraPayVariants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PremiumPerc",
                table: "ExtraPayVariants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MatSupportMultiplier",
                table: "ExtraPayVariants",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
