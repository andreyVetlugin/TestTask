using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class egissoV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KpCodeLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PrivilegeId = table.Column<Guid>(nullable: false),
                    KpCodeId = table.Column<Guid>(nullable: false),
                    MeasureUnitId = table.Column<Guid>(nullable: false),
                    Value = table.Column<double>(nullable: false),
                    EgissoId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpCodeLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KpCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 11, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    IsChildIsReason = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeasureUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PpNumber = table.Column<int>(nullable: false),
                    PositionCode = table.Column<string>(maxLength: 2, nullable: true),
                    Title = table.Column<string>(maxLength: 32, nullable: true),
                    ShortTitle = table.Column<string>(maxLength: 16, nullable: true),
                    IsDecimal = table.Column<bool>(nullable: false),
                    OkeiCode = table.Column<string>(maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasureUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PpNumber = table.Column<int>(nullable: false),
                    PositionCode = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    PeriodTypeId = table.Column<Guid>(nullable: false),
                    ProvisionFormId = table.Column<Guid>(nullable: false),
                    UsingNeedCriteria = table.Column<bool>(nullable: false),
                    Monetization = table.Column<bool>(nullable: false),
                    EgissoCode = table.Column<string>(maxLength: 4, nullable: true),
                    EgissoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvisionForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvisionForms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KpCodeLinks");

            migrationBuilder.DropTable(
                name: "KpCodes");

            migrationBuilder.DropTable(
                name: "MeasureUnits");

            migrationBuilder.DropTable(
                name: "PeriodTypes");

            migrationBuilder.DropTable(
                name: "Privileges");

            migrationBuilder.DropTable(
                name: "ProvisionForms");
        }
    }
}
