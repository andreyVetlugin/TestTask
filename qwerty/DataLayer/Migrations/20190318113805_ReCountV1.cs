using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class ReCountV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsElite",
                table: "Solutions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "RecountDebts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    Debt = table.Column<decimal>(nullable: false),
                    MonthlyPay = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecountDebts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecountDebts");

            migrationBuilder.DropColumn(
                name: "IsElite",
                table: "Solutions");
        }
    }
}
