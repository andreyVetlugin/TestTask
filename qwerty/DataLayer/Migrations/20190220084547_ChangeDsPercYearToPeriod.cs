using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class ChangeDsPercYearToPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "DsPercs");

            migrationBuilder.AddColumn<DateTime>(
                name: "Period",
                table: "DsPercs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "DsPercs");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "DsPercs",
                nullable: false,
                defaultValue: 0);
        }
    }
}
