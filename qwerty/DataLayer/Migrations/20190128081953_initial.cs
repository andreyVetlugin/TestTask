using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DsPercs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    AgeDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsPercs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraPays",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonRootId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    OutDate = table.Column<DateTime>(nullable: true),
                    VariantId = table.Column<Guid>(nullable: false),
                    UralMultiplier = table.Column<decimal>(nullable: false),
                    Salary = table.Column<decimal>(nullable: false),
                    Premium = table.Column<decimal>(nullable: false),
                    MaterialSupport = table.Column<decimal>(nullable: false),
                    Perks = table.Column<decimal>(nullable: false),
                    Vysluga = table.Column<decimal>(nullable: false),
                    Secrecy = table.Column<decimal>(nullable: false),
                    Qualification = table.Column<decimal>(nullable: false),
                    GosPension = table.Column<decimal>(nullable: false),
                    ExtraPension = table.Column<decimal>(nullable: false),
                    Ds = table.Column<decimal>(nullable: false),
                    DsPerc = table.Column<decimal>(nullable: false),
                    TotalExtraPay = table.Column<decimal>(nullable: false),
                    TotalPension = table.Column<decimal>(nullable: false),
                    TotalPensionAndExtraPay = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraPays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraPayVariants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    UralMultiplier = table.Column<decimal>(nullable: false),
                    PremiumPerc = table.Column<decimal>(nullable: false),
                    MatSupportMultiplier = table.Column<decimal>(nullable: false),
                    VyslugaMultiplier = table.Column<decimal>(nullable: false),
                    VyslugaDivPerc = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraPayVariants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetInfoLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ControllerName = table.Column<string>(nullable: true),
                    ActionName = table.Column<string>(nullable: true),
                    HttpMethod = table.Column<string>(nullable: true),
                    RequestBody = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetInfoLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GosPensionUpdates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Approved = table.Column<bool>(nullable: false),
                    Declined = table.Column<bool>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GosPensionUpdates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MinExtraPays",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MinExtraPays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganizationName = table.Column<string>(nullable: true),
                    Multiplier = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonBankCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonRootId = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    OutDate = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    ValidThru = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBankCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NextId = table.Column<Guid>(nullable: true),
                    RootId = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    OutdateTime = table.Column<DateTime>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    EmployeeTypeId = table.Column<Guid>(nullable: false),
                    DistrictId = table.Column<Guid>(nullable: false),
                    PensionTypeId = table.Column<Guid>(nullable: false),
                    PayoutTypeId = table.Column<Guid>(nullable: false),
                    PensionEndDate = table.Column<DateTime>(nullable: true),
                    AdditionalPensionId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    Birthplace = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Sex = table.Column<char>(nullable: false),
                    SNILS = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DocTypeId = table.Column<string>(nullable: true),
                    CodeEgisso = table.Column<string>(nullable: true),
                    DocNumber = table.Column<string>(nullable: true),
                    DocSeria = table.Column<string>(nullable: true),
                    Issuer = table.Column<string>(nullable: true),
                    IssueDate = table.Column<DateTime>(nullable: false),
                    PensionCaseNumber = table.Column<int>(nullable: false),
                    Approved = table.Column<bool>(nullable: false),
                    DocsSubmitDate = table.Column<DateTime>(nullable: true),
                    DocsDestinationDate = table.Column<DateTime>(nullable: true),
                    StoppedSolutions = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostInfoLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Operation = table.Column<byte>(nullable: false),
                    EntityRootId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostInfoLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReestrElements",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReestrId = table.Column<Guid>(nullable: false),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    PersonInfoId = table.Column<Guid>(nullable: false),
                    From = table.Column<DateTime>(nullable: true),
                    To = table.Column<DateTime>(nullable: true),
                    BaseSumm = table.Column<decimal>(nullable: false),
                    Summ = table.Column<decimal>(nullable: false),
                    PersonBankCardId = table.Column<Guid>(nullable: true),
                    Comment = table.Column<string>(maxLength: 256, nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReestrElements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reestrs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    InitDate = table.Column<DateTime>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reestrs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Permissions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUserLink",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserLink", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    OutdateTime = table.Column<DateTime>(nullable: true),
                    Destination = table.Column<DateTime>(nullable: false),
                    Execution = table.Column<DateTime>(nullable: false),
                    Type = table.Column<byte>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    TotalPension = table.Column<decimal>(nullable: false),
                    TotalExtraPay = table.Column<decimal>(nullable: false),
                    DS = table.Column<decimal>(nullable: false),
                    DSperc = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RootId = table.Column<Guid>(nullable: false),
                    NextId = table.Column<Guid>(nullable: true),
                    PersonInfoRootId = table.Column<Guid>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    OutdateTime = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    FunctionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DsPercs");

            migrationBuilder.DropTable(
                name: "ExtraPays");

            migrationBuilder.DropTable(
                name: "ExtraPayVariants");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "GetInfoLogs");

            migrationBuilder.DropTable(
                name: "GosPensionUpdates");

            migrationBuilder.DropTable(
                name: "MinExtraPays");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PersonBankCards");

            migrationBuilder.DropTable(
                name: "PersonInfos");

            migrationBuilder.DropTable(
                name: "PostInfoLogs");

            migrationBuilder.DropTable(
                name: "ReestrElements");

            migrationBuilder.DropTable(
                name: "Reestrs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RoleUserLink");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkInfos");
        }
    }
}
