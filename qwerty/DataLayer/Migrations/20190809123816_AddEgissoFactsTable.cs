using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class AddEgissoFactsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EgissoFacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    PersonInfoId = table.Column<Guid>(nullable: false),
                    SolutionId = table.Column<Guid>(nullable: false),
                    DecisionDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    PrivilegeId = table.Column<Guid>(nullable: false),
                    CategoryLinkId = table.Column<Guid>(nullable: false),
                    ProvisionFormId = table.Column<Guid>(nullable: false),
                    MeasureUnitId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EgissoFacts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EgissoFacts");
        }
    }
}
