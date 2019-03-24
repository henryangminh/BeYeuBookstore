using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ERPWebApp.Data.EF.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.RoleId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AR_GeneralJournal",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DebitAccount = table.Column<string>(type: "char(7)", nullable: true),
                    CreditAccount = table.Column<string>(type: "char(7)", nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Reference = table.Column<int>(nullable: true),
                    ReferenceDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_GeneralJournal", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "AR_InvoiceStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    ARInvoiceStatusName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_InvoiceStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "AR_SalesEvent",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Invoice_FK = table.Column<int>(nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomerAddressBook_FK = table.Column<int>(nullable: true),
                    CustomerPONumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    CustomerPODate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SalesPerson_No = table.Column<Guid>(nullable: true),
                    Sales_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Freight = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Tax_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Total_Invoice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_SalesEvent", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "BoqAcceptanceStatuses",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(maxLength: 15, nullable: true),
                    SOStatusName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqAcceptanceStatuses", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "BoqOrderStatuses",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(maxLength: 25, nullable: true),
                    SOStatusName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqOrderStatuses", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "EdittingForms",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormId = table.Column<int>(nullable: false),
                    TableId = table.Column<int>(nullable: false),
                    EditorFK = table.Column<int>(nullable: false),
                    TimeBegin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EdittingForms", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLogs",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorContent = table.Column<string>(nullable: true),
                    ErrorTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    FormName = table.Column<string>(maxLength: 500, nullable: true),
                    Level = table.Column<string>(maxLength: 25, nullable: true),
                    Type = table.Column<string>(maxLength: 30, nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLogs", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    KeyId = table.Column<string>(type: "varchar(128)", nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    URL = table.Column<string>(maxLength: 250, nullable: false),
                    ParentId = table.Column<string>(maxLength: 128, nullable: true),
                    IconCss = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_AccountDefault",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodeSelect = table.Column<string>(maxLength: 6, nullable: false),
                    DebitAccount = table.Column<string>(maxLength: 7, nullable: true),
                    CreditAccount = table.Column<string>(maxLength: 7, nullable: true),
                    note = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_AccountDefault", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_ARTransactionType",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    AccountName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_ARTransactionType", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_City",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CityName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_City", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_Ledger",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account_No = table.Column<string>(maxLength: 7, nullable: true),
                    Account_Name = table.Column<string>(maxLength: 255, nullable: true),
                    Account_Class = table.Column<string>(type: "char(1)", nullable: true),
                    Account_Level = table.Column<string>(type: "char(1)", nullable: true),
                    Account_Liabilities = table.Column<bool>(nullable: true),
                    Account_Detail = table.Column<bool>(nullable: true),
                    ParentAccount_No = table.Column<string>(type: "char(7)", nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Ledger", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_Nation",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NationName = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Nation", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_PaymentTerms",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PaymentTermsName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_PaymentTerms", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_PaymentTypes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PaymentTypeName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_PaymentTypes", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_Period",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeriodId = table.Column<string>(unicode: false, maxLength: 4, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PeriodStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Period", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_ProductType",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    ProductTypeName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_ProductType", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_Region",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameRegion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Region", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_Religion",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReligionName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Religion", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GN_ShipVia",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_ShipVia", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GnProduct_Attributes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 25, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    is_required = table.Column<bool>(nullable: false),
                    is_unique = table.Column<bool>(nullable: false),
                    note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GnProduct_Attributes", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "GNProjectStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusID = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    SOStatusName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GNProjectStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_AllowedVacationStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HPStatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_AllowedVacationStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Department",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Department_Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Department", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_EducationalLevel",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EducationLevelName = table.Column<string>(maxLength: 50, nullable: true),
                    Descriontion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_EducationalLevel", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Enterprise",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EnterpriseName = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Enterprise", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_IncomeTaxList",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IncomeID = table.Column<string>(maxLength: 4, nullable: true),
                    IncomeName = table.Column<string>(maxLength: 30, nullable: true),
                    CloseBelow = table.Column<decimal>(nullable: true),
                    CloseUpper = table.Column<decimal>(nullable: true),
                    Tax = table.Column<int>(nullable: false),
                    Subtract = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_IncomeTaxList", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Language",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LanguageName = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Language", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Major",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MajorName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Major", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Nationality",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NationalityName = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Nationality", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Position",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PositionName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Position", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_Qualification",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QualificationName = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Qualification", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_TimekeepingType",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimekeepingTypeName = table.Column<string>(maxLength: 200, nullable: true),
                    GetPaidRate = table.Column<double>(nullable: true),
                    GetBasicSalary = table.Column<bool>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_TimekeepingType", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_TrainingSystem",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrainingSystemName = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_TrainingSystem", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HP_WorkingGroup",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WorkingGroupName = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_WorkingGroup", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HpEmployeeTypes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HpEmployeeTypes", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "HpQuotas",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuotaID = table.Column<string>(maxLength: 10, nullable: true),
                    QuotaName = table.Column<string>(maxLength: 45, nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HpQuotas", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "IC_Category",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    CategoryText = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_Category", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "IC_Warehouse",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    WarehouseName = table.Column<string>(maxLength: 50, nullable: true),
                    WarehouseAddress = table.Column<string>(maxLength: 100, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_Warehouse", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "PO_Notification",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Vendor_FK = table.Column<int>(nullable: true),
                    PO_ID = table.Column<int>(nullable: true),
                    PO_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Ship_No = table.Column<string>(nullable: true),
                    Ship_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Receiver_FK = table.Column<int>(nullable: true),
                    BillOf_Loading_No = table.Column<string>(nullable: true),
                    Invoice_No = table.Column<int>(nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    StattusCode = table.Column<int>(nullable: false),
                    Notification_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notification_LastUpdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_Notification", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "PO_OrderStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    POStatus_ID = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    POStatusText = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_OrderStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SO_AcceptanceStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    SOStatusName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_AcceptanceStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_NotificationGeneral",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificationContent = table.Column<string>(maxLength: 500, nullable: true),
                    Referent = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_NotificationGeneral", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_OrderDetailStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    SODetailStatusName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_OrderDetailStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_OrderStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    SOStatusName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_OrderStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_PaymentStatus",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    PaymentStatusName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_PaymentStatus", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_Report",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReportName = table.Column<string>(maxLength: 200, nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_Report", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_SystemParameters",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Fax = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MinimumSalary = table.Column<decimal>(nullable: true),
                    HolidaysInYear = table.Column<string>(nullable: true),
                    CorporateSocialInsurance = table.Column<double>(nullable: true),
                    EmployeeSocialInsurance = table.Column<double>(nullable: true),
                    CorporateMedicalInsurance = table.Column<double>(nullable: true),
                    EmployeeMedicalInsurance = table.Column<double>(nullable: true),
                    CorparateUnemploymentInsurance = table.Column<double>(nullable: true),
                    EmployeeUnemploymentInsurance = table.Column<double>(nullable: true),
                    CorparateUnionFee = table.Column<double>(nullable: true),
                    EmployeeUnionFee = table.Column<double>(nullable: true),
                    TotalSalaryFund = table.Column<decimal>(nullable: true),
                    YourselfDeduction = table.Column<decimal>(nullable: true),
                    DependencyDeduction = table.Column<decimal>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_SystemParameters", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "tblMenu",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuValue = table.Column<string>(maxLength: 1000, nullable: true),
                    FormName = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    FormParentName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMenu", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    Dob = table.Column<DateTime>(nullable: true),
                    IdNumber = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    IdDate = table.Column<DateTime>(nullable: true),
                    TaxIDNumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Street = table.Column<string>(maxLength: 200, nullable: true),
                    Ward = table.Column<string>(maxLength: 100, nullable: true),
                    District = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    Country = table.Column<string>(maxLength: 100, nullable: true),
                    Origin_FK = table.Column<int>(nullable: true),
                    LastLogIn = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Fax = table.Column<string>(unicode: false, maxLength: 25, nullable: true),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    IsCustomer = table.Column<bool>(nullable: true),
                    IsVendor = table.Column<bool>(nullable: true),
                    IsEmployee = table.Column<bool>(nullable: true),
                    IsPersonal = table.Column<bool>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    LastupdatedFk = table.Column<Guid>(nullable: true),
                    LastupdatedName = table.Column<string>(maxLength: 200, nullable: true),
                    GnCityKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_GN_City_GnCityKeyId",
                        column: x => x.GnCityKeyId,
                        principalTable: "GN_City",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GL_GeneralLedger",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Account_FK = table.Column<int>(nullable: false),
                    Account_No = table.Column<string>(maxLength: 7, nullable: true),
                    NormalBalance = table.Column<string>(type: "char(1)", nullable: true),
                    BeginBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalDebits = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalCredits = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PeriodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GL_GeneralLedger", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.GL_GeneralLedger_dbo.GN_Ledger_Account_FK",
                        column: x => x.Account_FK,
                        principalTable: "GN_Ledger",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.GL_GeneralLedger_dbo.GN_Period_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "GN_Period",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GnProduct_Attribute_Values",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    productAttributeFk = table.Column<int>(nullable: false),
                    code = table.Column<string>(maxLength: 25, nullable: true),
                    ValueName = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GnProduct_Attribute_Values", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_GnProduct_Attribute_Values_GnProduct_Attributes_productAttri~",
                        column: x => x.productAttributeFk,
                        principalTable: "GnProduct_Attributes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_EmployeeLog",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChangeCode = table.Column<string>(nullable: true),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    ChangeDepartmentFK = table.Column<int>(nullable: false),
                    ChangeWorkingGroupFK = table.Column<int>(nullable: false),
                    ChangeContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_EmployeeLog", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_EmployeeLog_HP_Department_ChangeDepartmentFK",
                        column: x => x.ChangeDepartmentFK,
                        principalTable: "HP_Department",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HP_EmployeeLog_HP_WorkingGroup_ChangeWorkingGroupFK",
                        column: x => x.ChangeWorkingGroupFK,
                        principalTable: "HP_WorkingGroup",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_Rank",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuotaFK = table.Column<int>(nullable: false),
                    RankName = table.Column<string>(maxLength: 50, nullable: true),
                    CoefficientsSalary = table.Column<double>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Rank", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_Rank_HpQuotas_QuotaFK",
                        column: x => x.QuotaFK,
                        principalTable: "HpQuotas",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(nullable: false),
                    FunctionId = table.Column<string>(maxLength: 128, nullable: false),
                    CanCreate = table.Column<bool>(nullable: false),
                    CanRead = table.Column<bool>(nullable: false),
                    CanConfirm = table.Column<bool>(nullable: false),
                    CanUpdate = table.Column<bool>(nullable: false),
                    CanDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_Permissions_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SO_NotificationGeneralDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Notification_FK = table.Column<int>(nullable: true),
                    User_FK = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_NotificationGeneralDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_NotificationGeneralDetail_dbo.SO_NotificationGeneral_Notification_FK",
                        column: x => x.Notification_FK,
                        principalTable: "SO_NotificationGeneral",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblMenuGroup",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuId = table.Column<int>(nullable: false),
                    GroupID = table.Column<int>(nullable: false),
                    levelAccess = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMenuGroup", x => new { x.MenuId, x.GroupID });
                    table.UniqueConstraint("AK_tblMenuGroup_KeyId", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.tblMenuGroup_dbo.Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.tblMenuGroup_dbo.tblMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "tblMenu",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AR_AccountsReceivableAdjustments",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Voucher_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Voucher_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Customer_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Tran_Type = table.Column<int>(nullable: true),
                    Invoice_No = table.Column<string>(maxLength: 15, nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Debit_Account = table.Column<string>(type: "char(7)", nullable: true),
                    Credit_Account = table.Column<string>(type: "char(7)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Authorized_By = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_AccountsReceivableAdjustments", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_AccountsReceivableAdjustments_dbo.User_Authorized_By",
                        column: x => x.Authorized_By,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AR_AccountsReceivableAdjustments_dbo.GN_ARTransactionType_Tran_Type",
                        column: x => x.Tran_Type,
                        principalTable: "GN_ARTransactionType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CR_CashReceipts",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Receipt_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Deposit_No = table.Column<string>(maxLength: 15, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Customer_FK = table.Column<int>(nullable: true),
                    FormType = table.Column<bool>(nullable: true),
                    Receipt_Type = table.Column<int>(nullable: true),
                    Check_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Check_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedBy_FK = table.Column<Guid>(nullable: true),
                    LastUpdatedBy_FK = table.Column<Guid>(nullable: true),
                    Payment = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_CashReceipts", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.CR_CashReceipts_dbo.User_CreatedBy_FK",
                        column: x => x.CreatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.CR_CashReceipts_dbo.User_LastUpdatedBy_FK",
                        column: x => x.LastUpdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IC_UnitCategory",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitCategoryName = table.Column<string>(maxLength: 20, nullable: false),
                    UnitCategoryDescription = table.Column<string>(maxLength: 255, nullable: true),
                    LastUpdatedBy = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_UnitCategory", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.IC_UnitCategory_dbo.User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_Vendor",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    UserFk = table.Column<Guid>(nullable: true),
                    Performance_Rating = table.Column<int>(nullable: true),
                    AP_Balance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Outstand_Inv_Amt = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Outstand_Credit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Last_Purchase_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Last_Payment_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    MTD_Purchase = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    YTD_Purchase = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LYR_Purchase = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    LastUpdatedFk = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_Vendor", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_PO_Vendor_User_LastUpdatedFk",
                        column: x => x.LastUpdatedFk,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PO_Vendor_User_UserFk",
                        column: x => x.UserFk,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SO_Customer",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(maxLength: 15, nullable: true),
                    UserFk = table.Column<Guid>(nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LastRevisedCreditLimitDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomerType = table.Column<int>(nullable: false),
                    QtyContructions = table.Column<int>(nullable: false),
                    EvaluatedCustomer = table.Column<string>(maxLength: 255, nullable: true),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    LastUpdatedBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_Customer", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Customer_dbo.User_LastUpdatedBy_FK",
                        column: x => x.LastUpdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_Customer_User_UserFk",
                        column: x => x.UserFk,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    GroupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.UserID, x.GroupID });
                    table.ForeignKey(
                        name: "FK_dbo.UserGroup_dbo.Group_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Group",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.UserGroup_dbo.User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CR_CashReceiptsAccountDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Receipt_FK = table.Column<int>(nullable: true),
                    DebitAccount = table.Column<string>(unicode: false, maxLength: 7, nullable: true),
                    CreditAccount = table.Column<string>(unicode: false, maxLength: 7, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_CashReceiptsAccountDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.CR_CashReceiptsAccountDetail_dbo.CR_CashReceipts_Receipt_FK",
                        column: x => x.Receipt_FK,
                        principalTable: "CR_CashReceipts",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CR_CashReceiptsDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Receipt_FK = table.Column<int>(nullable: true),
                    Invoice_No = table.Column<int>(nullable: true),
                    PeriodName = table.Column<string>(maxLength: 10, nullable: true),
                    PercentPayment = table.Column<double>(nullable: false),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Deposit_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Invoice_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AccumulatedLastAmountAcc = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CollectedAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    RemainedAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_CashReceiptsDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.CR_CashReceiptsDetail_dbo.CR_CashReceipts_Receipt_FK",
                        column: x => x.Receipt_FK,
                        principalTable: "CR_CashReceipts",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IC_Unit",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitName = table.Column<string>(maxLength: 20, nullable: false),
                    UnitCategory_FK = table.Column<int>(nullable: true),
                    Factor = table.Column<double>(nullable: true),
                    Rounding = table.Column<int>(nullable: true),
                    LastUpdatedBy = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_Unit", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Unit_dbo.User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Unit_dbo.IC_UnitCategory_UnitCategory_FK",
                        column: x => x.UnitCategory_FK,
                        principalTable: "IC_UnitCategory",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_AccountPayable",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AP_No = table.Column<string>(unicode: false, maxLength: 7, nullable: true),
                    Vendor_FK = table.Column<int>(nullable: true),
                    Transaction_No = table.Column<int>(nullable: false),
                    Record_Code = table.Column<string>(maxLength: 2, nullable: true),
                    PO_No = table.Column<int>(nullable: false),
                    Transact_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ConstructionId = table.Column<string>(unicode: false, maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_AccountPayable", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_AccountPayable_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_AccountsPayableAdjustments",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Voucher_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Voucher_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Vendor_FK = table.Column<int>(nullable: true),
                    Transaction_ID = table.Column<string>(maxLength: 2, nullable: true),
                    Invoice_No = table.Column<int>(nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Debit_Account = table.Column<string>(maxLength: 7, nullable: true),
                    Credit_Account = table.Column<string>(maxLength: 7, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Authorized_By = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_AccountsPayableAdjustments", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_AccountsPayableAdjustments_dbo.User_Authorized_By",
                        column: x => x.Authorized_By,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.PO_AccountsPayableAdjustments_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_APInvoices",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Vendor_FK = table.Column<int>(nullable: false),
                    Invoice_No = table.Column<int>(nullable: false),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    PO_ID = table.Column<int>(nullable: false),
                    PO_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Invoice_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Discount_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Due_Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_APInvoices", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_APInvoices_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO_CashDisbursement",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PO_CashDisb_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    PO_CashDisb_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Vendor_FK = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Disbursement_Type = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    Gross_Inv_Amt = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Discount_Amt = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Check_No = table.Column<string>(maxLength: 15, nullable: true),
                    Check_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DebitAccount = table.Column<string>(maxLength: 7, nullable: true),
                    CreditAccount = table.Column<string>(maxLength: 7, nullable: true),
                    Payment = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_CashDisbursement", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_CashDisbursement_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GNProject",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConstructionId = table.Column<string>(maxLength: 25, nullable: false),
                    ConstructionName = table.Column<string>(maxLength: 500, nullable: true),
                    ProjectName = table.Column<string>(maxLength: 500, nullable: true),
                    EscrowAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    WarrantyAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    WarrantyDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CustomerFK = table.Column<int>(nullable: false),
                    SalesOrderId = table.Column<int>(nullable: true),
                    Region_FK = table.Column<int>(nullable: true),
                    LastUpdatedBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GNProject", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.GNProject_dbo.SO_Customer_CustomerFK",
                        column: x => x.CustomerFK,
                        principalTable: "SO_Customer",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GNProject_User_LastUpdatedBy_FK",
                        column: x => x.LastUpdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.GNProject_dbo.GN_Region_Region_FK",
                        column: x => x.Region_FK,
                        principalTable: "GN_Region",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.GNProject_dbo.GNProjectStatus_Status",
                        column: x => x.Status,
                        principalTable: "GNProjectStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GN_Product",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductCode = table.Column<string>(maxLength: 15, nullable: true),
                    ProductTypeFK = table.Column<int>(nullable: true),
                    Product_Name = table.Column<string>(maxLength: 500, nullable: true),
                    Product_Unit = table.Column<int>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Is_simple = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    IsOutsourcing = table.Column<bool>(nullable: true),
                    Image = table.Column<string>(maxLength: 255, nullable: true),
                    LastUpdateBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_Product", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.GN_Product_dbo.User_LastUpdateBy_FK",
                        column: x => x.LastUpdateBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.GN_Product_dbo.GN_ProductType_Product_Type",
                        column: x => x.ProductTypeFK,
                        principalTable: "GN_ProductType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.GN_Product_dbo.IC_Unit_Product_Unit",
                        column: x => x.Product_Unit,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_APInvoicesDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Invoice_FK = table.Column<int>(nullable: false),
                    Inventory_FK = table.Column<string>(unicode: false, nullable: true),
                    Unit_FK = table.Column<int>(nullable: true),
                    Qty_Ordered = table.Column<double>(nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_APInvoicesDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_APInvoicesDetail_dbo.PO_APInvoices_Invoice_FK",
                        column: x => x.Invoice_FK,
                        principalTable: "PO_APInvoices",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO_CashDisbursementDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PO_CashDisb_No_FK = table.Column<int>(nullable: false),
                    Invoice_FK = table.Column<int>(nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Invoice_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_CashDisbursementDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_CashDisbursementDetail_dbo.PO_CashDisbursement_PO_CashDisb_No_FK",
                        column: x => x.PO_CashDisb_No_FK,
                        principalTable: "PO_CashDisbursement",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AR_AccountsReceivable",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Transact_No = table.Column<int>(nullable: true),
                    Transaction_Date = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    AR_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Customer_FK = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Record_Code = table.Column<string>(type: "char(2)", nullable: true),
                    PO_No = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Warehouse_FK = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Department = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Type = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_AccountsReceivable", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_AccountsReceivable_dbo.GNProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AR_PeriodPayment",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PeriodName = table.Column<string>(maxLength: 25, nullable: false),
                    PaymentPercent = table.Column<double>(nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Payment_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    UnpaidAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true),
                    Project_FK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_PeriodPayment", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_PeriodPayment_dbo.GNProject_Project_FK",
                        column: x => x.Project_FK,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectFK = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Link = table.Column<string>(maxLength: 500, nullable: true),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.Document_dbo.GNProject_ProjectFK",
                        column: x => x.ProjectFK,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GN_CustomerLiabilities",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Project_FK = table.Column<int>(nullable: false),
                    ContractAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IncurredAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SettlementAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ReceivableAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GN_CustomerLiabilities", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.GN_CustomerLiabilities_dbo.GNProject_Project_FK",
                        column: x => x.Project_FK,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GNProjectDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectFK = table.Column<int>(nullable: true),
                    SO_FK = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GNProjectDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.GNProjectDetails_dbo.GNProject_ProjectFK",
                        column: x => x.ProjectFK,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProject",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProject", x => new { x.UserID, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_dbo.UserProject_dbo.GNProject_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.UserProject_dbo.User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GnProduct_Links",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_id = table.Column<int>(nullable: false),
                    Linked_product_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GnProduct_Links", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_GnProduct_Links_GN_Product_Linked_product_id",
                        column: x => x.Linked_product_id,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GnProduct_Links_GN_Product_Product_id",
                        column: x => x.Product_id,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GnProduct_ProductAttributes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    product_fk = table.Column<int>(nullable: false),
                    Product_AttributeFk = table.Column<int>(nullable: false),
                    Attribute_value = table.Column<string>(maxLength: 2000, nullable: true),
                    price_extra = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GnProduct_ProductAttributes", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_GnProduct_ProductAttributes_GnProduct_Attributes_Product_Att~",
                        column: x => x.Product_AttributeFk,
                        principalTable: "GnProduct_Attributes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GnProduct_ProductAttributes_GN_Product_product_fk",
                        column: x => x.product_fk,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IC_Inventory",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Warehouse_FK = table.Column<int>(nullable: true),
                    Product_FK = table.Column<int>(nullable: true),
                    FixedAsset = table.Column<bool>(nullable: true),
                    Tax_Rate = table.Column<double>(nullable: true),
                    ROP = table.Column<int>(nullable: true),
                    EOQ = table.Column<int>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: true),
                    Length = table.Column<double>(nullable: true),
                    Width = table.Column<double>(nullable: true),
                    Height = table.Column<double>(nullable: true),
                    GL_Asset = table.Column<string>(maxLength: 10, nullable: true),
                    GL_COGS = table.Column<string>(maxLength: 10, nullable: true),
                    GL_Sales = table.Column<string>(maxLength: 10, nullable: true),
                    GL_NonTaxSales = table.Column<string>(maxLength: 10, nullable: true),
                    LastSaleDate = table.Column<DateTime>(nullable: true),
                    LastPODate = table.Column<DateTime>(nullable: true),
                    LastReceiptDate = table.Column<DateTime>(nullable: true),
                    LeadTime = table.Column<int>(nullable: true),
                    LastCost = table.Column<decimal>(nullable: false),
                    AverageCost = table.Column<decimal>(nullable: false),
                    Qty_On_Order = table.Column<int>(nullable: true),
                    Qty_In_Stock = table.Column<int>(nullable: true),
                    Qty_On_Hand = table.Column<int>(nullable: true),
                    Qty_Allocated = table.Column<int>(nullable: true),
                    SafetyStock = table.Column<int>(nullable: true),
                    TotalInventoryValue = table.Column<decimal>(nullable: false),
                    Location = table.Column<string>(maxLength: 100, nullable: true),
                    VendorNumber_FK = table.Column<int>(nullable: true),
                    LastUpdateBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IcCategoryKeyId = table.Column<int>(nullable: true),
                    IcUnitKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_Inventory", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_IC_Inventory_IC_Category_IcCategoryKeyId",
                        column: x => x.IcCategoryKeyId,
                        principalTable: "IC_Category",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IC_Inventory_IC_Unit_IcUnitKeyId",
                        column: x => x.IcUnitKeyId,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Inventory_dbo.GN_AddressBook_LastUpdateBy_FK",
                        column: x => x.LastUpdateBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Inventory_dbo.GN_Product_Product_FK",
                        column: x => x.Product_FK,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Inventory_dbo.PO_Vendor_VendorNumber_FK",
                        column: x => x.VendorNumber_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.IC_Inventory_dbo.IC_Warehouse_Warehouse_FK",
                        column: x => x.Warehouse_FK,
                        principalTable: "IC_Warehouse",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MRP_BOM",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductBOM_FK = table.Column<int>(nullable: true),
                    BOMofTotalRef_FK = table.Column<string>(nullable: true),
                    Product_FK = table.Column<int>(nullable: true),
                    Product_Qty = table.Column<double>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MRP_BOM", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_MRP_BOM_GN_Product_ProductBOM_FK",
                        column: x => x.ProductBOM_FK,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MRP_BOM_GN_Product_Product_FK",
                        column: x => x.Product_FK,
                        principalTable: "GN_Product",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MRP_BOM_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AR_PeriodPaymentDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    PeriodPayment_FK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_PeriodPaymentDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_PeriodPaymentDetail_dbo.AR_PeriodPayment_PeriodPayment_FK",
                        column: x => x.PeriodPayment_FK,
                        principalTable: "AR_PeriodPayment",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IC_InventoryCustomerPrice",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InventoryFk = table.Column<int>(nullable: false),
                    CustomerFk = table.Column<int>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_InventoryCustomerPrice", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_IC_InventoryCustomerPrice_SO_Customer_CustomerFk",
                        column: x => x.CustomerFk,
                        principalTable: "SO_Customer",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IC_InventoryCustomerPrice_IC_Inventory_InventoryFk",
                        column: x => x.InventoryFk,
                        principalTable: "IC_Inventory",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AR_InvoiceDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Invoice_FK = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Inventory_FK = table.Column<int>(nullable: true),
                    Unit_FK = table.Column<int>(nullable: true),
                    Unit_Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    QtyOrderedNow = table.Column<int>(nullable: true),
                    Qty_Shipped = table.Column<int>(nullable: true),
                    QtyBackOrdered = table.Column<int>(nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DiscountPercent = table.Column<double>(nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TaxRate = table.Column<double>(nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_InvoiceDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_InvoiceDetail_dbo.IC_Inventory_Inventory_FK",
                        column: x => x.Inventory_FK,
                        principalTable: "IC_Inventory",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AR_InvoiceDetail_dbo.IC_Unit_Unit_FK",
                        column: x => x.Unit_FK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CR_RemittanceAdvice",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RA_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    RA_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Customer_No = table.Column<int>(nullable: false),
                    RA_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Invoice_No = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CR_RemittanceAdvice", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SO_Notification",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Customer_FK = table.Column<int>(nullable: false),
                    SO_ID = table.Column<int>(nullable: false),
                    SODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Acceptance_FK = table.Column<int>(nullable: true),
                    Acceptance_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Engineer_FK = table.Column<int>(nullable: true),
                    Payment_FK = table.Column<int>(nullable: true),
                    Payment_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatePaymentBy_FK = table.Column<int>(nullable: true),
                    PaymentStatus_FK = table.Column<int>(nullable: true),
                    Invoice_No = table.Column<int>(nullable: true),
                    Notification_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Notification_LastUpdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_Notification", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Notification_dbo.SO_Customer_Customer_FK",
                        column: x => x.Customer_FK,
                        principalTable: "SO_Customer",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Notification_dbo.SO_PaymentStatus_PaymentStatus_FK",
                        column: x => x.PaymentStatus_FK,
                        principalTable: "SO_PaymentStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AR_Invoice",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    InvoiceID = table.Column<string>(maxLength: 20, nullable: true),
                    SO_ID = table.Column<int>(nullable: true),
                    Customer_FK = table.Column<int>(nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    PaymentTerm_FK = table.Column<int>(nullable: true),
                    Type = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    CustomerPONumber = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    CustomerPODate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Salesperson_No = table.Column<Guid>(nullable: true),
                    Sales_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    GL_Account = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    DebitOrCredit = table.Column<string>(type: "char(1)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    UnpaidAmount = table.Column<decimal>(nullable: false),
                    Freight = table.Column<decimal>(nullable: false),
                    Discount = table.Column<decimal>(nullable: false),
                    Total_Tax = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Total_Invoice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false),
                    ARInvoiceStatus_FK = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AR_Invoice", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AR_Invoice_dbo.AR_InvoiceStatus_ARInvoiceStatus_FK",
                        column: x => x.ARInvoiceStatus_FK,
                        principalTable: "AR_InvoiceStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AR_Invoice_dbo.SO_Customer_Customer_FK",
                        column: x => x.Customer_FK,
                        principalTable: "SO_Customer",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AR_Invoice_dbo.GN_PaymentTerms_PaymentTerm_FK",
                        column: x => x.PaymentTerm_FK,
                        principalTable: "GN_PaymentTerms",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AR_Invoice_dbo.HP_Employee_Salesperson_No",
                        column: x => x.Salesperson_No,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoqAcceptanceDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Acceptance_FK = table.Column<int>(nullable: false),
                    OrderDetail_FK = table.Column<int>(nullable: true),
                    Unit_Name = table.Column<string>(nullable: true),
                    Unit_FK = table.Column<int>(nullable: true),
                    QtyOrdered = table.Column<double>(nullable: true),
                    AccumulatedLastPercentAcc = table.Column<double>(nullable: true),
                    AccumulatedLastQtyAcc = table.Column<double>(nullable: true),
                    PercentAcceptanceNow = table.Column<double>(nullable: true),
                    QtyAcceptanceNow = table.Column<double>(nullable: true),
                    AccumulatedNowPercentAcc = table.Column<double>(nullable: true),
                    AccumulatedNowQtyAcc = table.Column<double>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    AccumulatedLastBillAcc = table.Column<decimal>(nullable: false),
                    Subtotal = table.Column<decimal>(nullable: false),
                    AccumulatedNowBillAcc = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqAcceptanceDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqAcceptanceDetails_IC_Unit_Unit_FK",
                        column: x => x.Unit_FK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoqAcceptances",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Acceptance_ID = table.Column<string>(maxLength: 50, nullable: true),
                    PaymentPeriod = table.Column<int>(nullable: true),
                    Acceptance_Date = table.Column<DateTime>(nullable: true),
                    SO_FK = table.Column<int>(nullable: true),
                    Engineer_FK = table.Column<Guid>(nullable: true),
                    AcceptanceStatus_FK = table.Column<int>(nullable: true),
                    Payment_ID = table.Column<string>(maxLength: 15, nullable: true),
                    CreatePaymentBy_FK = table.Column<Guid>(nullable: true),
                    Total = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqAcceptances", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqAcceptances_BoqAcceptanceStatuses_AcceptanceStatus_FK",
                        column: x => x.AcceptanceStatus_FK,
                        principalTable: "BoqAcceptanceStatuses",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqAcceptances_User_CreatePaymentBy_FK",
                        column: x => x.CreatePaymentBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqAcceptances_User_Engineer_FK",
                        column: x => x.Engineer_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoqOrderDetailBoms",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderDetailFK = table.Column<int>(nullable: false),
                    BOMofTotal_FK = table.Column<string>(nullable: true),
                    BomCodeFK = table.Column<string>(nullable: false),
                    BomDetailFK = table.Column<string>(maxLength: 15, nullable: true),
                    NameBomdetail = table.Column<string>(maxLength: 500, nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    Qty = table.Column<double>(nullable: true),
                    Product_Type = table.Column<int>(nullable: true),
                    PriceB1 = table.Column<decimal>(nullable: false),
                    SubTotalB1 = table.Column<decimal>(nullable: false),
                    Discount_Rate = table.Column<double>(nullable: true),
                    PriceB1_Old = table.Column<decimal>(nullable: false),
                    PriceB2 = table.Column<decimal>(nullable: false),
                    SubTotalB2 = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqOrderDetailBoms", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqOrderDetailBoms_GN_ProductType_Product_Type",
                        column: x => x.Product_Type,
                        principalTable: "GN_ProductType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqOrderDetailBoms_IC_Unit_UnitFK",
                        column: x => x.UnitFK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoqSalesOrders",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SODate = table.Column<DateTime>(nullable: false),
                    SOStatus_FK = table.Column<int>(nullable: false),
                    ProjectFK = table.Column<int>(nullable: false),
                    CustomerPONumber = table.Column<string>(maxLength: 20, nullable: true),
                    Version_name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    SO_ref = table.Column<string>(maxLength: 25, nullable: true),
                    GeneralExpensesRate = table.Column<double>(nullable: true),
                    ProfitRate = table.Column<double>(nullable: true),
                    TaxRate = table.Column<double>(nullable: true),
                    Discount_K1 = table.Column<double>(nullable: true),
                    Qty_K2 = table.Column<double>(nullable: true),
                    Price_K3 = table.Column<double>(nullable: true),
                    Provision_K4 = table.Column<double>(nullable: true),
                    Provision_Amount = table.Column<decimal>(nullable: false),
                    Discount_Amt = table.Column<decimal>(nullable: false),
                    Provision_AmountUsed = table.Column<decimal>(nullable: false),
                    GeneralExpensesAmount = table.Column<decimal>(nullable: false),
                    ProfitAmount = table.Column<decimal>(nullable: false),
                    Tax_Amount = table.Column<decimal>(nullable: false),
                    Subtotal = table.Column<decimal>(nullable: false),
                    Total_Order = table.Column<decimal>(nullable: false),
                    GeneralExpensesAmountBOQ2 = table.Column<decimal>(nullable: false),
                    ProfitAmountBOQ2 = table.Column<decimal>(nullable: false),
                    Tax_AmountBOQ2 = table.Column<decimal>(nullable: false),
                    SubtotalBOQ2 = table.Column<decimal>(nullable: false),
                    Total_OrderBOQ2 = table.Column<decimal>(nullable: false),
                    DateRequestBOQ2 = table.Column<DateTime>(nullable: true),
                    Ndate = table.Column<int>(nullable: true),
                    maxIdOrderdetail = table.Column<int>(nullable: false),
                    ScheduleStartDate = table.Column<DateTime>(nullable: true),
                    ScheduleFinishDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    CustomerPODate = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    SalesPerson_FK = table.Column<int>(nullable: true),
                    CreatedBy_FK = table.Column<Guid>(nullable: true),
                    LastUpdatedBy_FK = table.Column<Guid>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy_FKBOQ2 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqSalesOrders", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqSalesOrders_User_CreatedBy_FK",
                        column: x => x.CreatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqSalesOrders_User_LastUpdatedBy_FK",
                        column: x => x.LastUpdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqSalesOrders_User_LastUpdatedBy_FKBOQ2",
                        column: x => x.LastUpdatedBy_FKBOQ2,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqSalesOrders_GNProject_ProjectFK",
                        column: x => x.ProjectFK,
                        principalTable: "GNProject",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoqSalesOrders_BoqOrderStatuses_SOStatus_FK",
                        column: x => x.SOStatus_FK,
                        principalTable: "BoqOrderStatuses",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoqOrderDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    stt = table.Column<int>(nullable: true),
                    IdRow = table.Column<int>(nullable: false),
                    Parent_Id = table.Column<int>(nullable: true),
                    LevelHeader = table.Column<int>(nullable: true),
                    NameHeader = table.Column<string>(maxLength: 15, nullable: true),
                    SODetailStatus_FK = table.Column<int>(nullable: false),
                    SO_FK = table.Column<int>(nullable: false),
                    BOMCode_FK = table.Column<string>(maxLength: 25, nullable: false),
                    Product_Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    QtyOrdered = table.Column<double>(nullable: true),
                    TotalQtyOrdered = table.Column<double>(nullable: false),
                    Material_Price = table.Column<decimal>(nullable: false),
                    Labor_Price = table.Column<decimal>(nullable: false),
                    Equipment_Price = table.Column<decimal>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    Subtotal = table.Column<decimal>(nullable: false),
                    QtyB2 = table.Column<double>(nullable: true),
                    Material_PriceB2 = table.Column<decimal>(nullable: false),
                    Labor_PriceB2 = table.Column<decimal>(nullable: false),
                    Equipment_PriceB2 = table.Column<decimal>(nullable: false),
                    TotalPriceB2 = table.Column<decimal>(nullable: false),
                    SubtotalB2 = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    IcUnitKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqOrderDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqOrderDetails_IC_Unit_IcUnitKeyId",
                        column: x => x.IcUnitKeyId,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqOrderDetails_BoqSalesOrders_SO_FK",
                        column: x => x.SO_FK,
                        principalTable: "BoqSalesOrders",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoqOrderDetails_IC_Unit_UnitFK",
                        column: x => x.UnitFK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoqSyntheticMaterials",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaleOrderFK = table.Column<int>(nullable: false),
                    BomDetailFK = table.Column<string>(maxLength: 15, nullable: true),
                    NameBomdetail = table.Column<string>(maxLength: 500, nullable: true),
                    UnitFK = table.Column<int>(nullable: false),
                    UnitName = table.Column<string>(nullable: true),
                    Product_Type = table.Column<int>(nullable: true),
                    PriceB1 = table.Column<decimal>(nullable: false),
                    PriceB2 = table.Column<decimal>(nullable: false),
                    TotalQtyB1 = table.Column<double>(nullable: false),
                    TotalQtyB2 = table.Column<double>(nullable: false),
                    TotalQtyOrdered = table.Column<double>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoqSyntheticMaterials", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_BoqSyntheticMaterials_GN_ProductType_Product_Type",
                        column: x => x.Product_Type,
                        principalTable: "GN_ProductType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoqSyntheticMaterials_BoqSalesOrders_SaleOrderFK",
                        column: x => x.SaleOrderFK,
                        principalTable: "BoqSalesOrders",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoqSyntheticMaterials_IC_Unit_UnitFK",
                        column: x => x.UnitFK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_Employee",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(maxLength: 15, nullable: true),
                    IdCard = table.Column<string>(nullable: true),
                    EmployeeTypeFK = table.Column<int>(nullable: true),
                    Birthplace = table.Column<int>(nullable: true),
                    Origin_FK = table.Column<int>(nullable: true),
                    Nation_FK = table.Column<int>(nullable: true),
                    Religion_FK = table.Column<int>(nullable: true),
                    NationalityFk = table.Column<int>(nullable: true),
                    IDDate = table.Column<DateTime>(nullable: true),
                    IDPlace = table.Column<string>(maxLength: 25, nullable: true),
                    PermanentResidence = table.Column<string>(maxLength: 50, nullable: true),
                    DistrictPR = table.Column<string>(maxLength: 30, nullable: true),
                    ProvincePRFk = table.Column<int>(nullable: true),
                    AccommodationCurrent = table.Column<string>(maxLength: 50, nullable: true),
                    DistrictAC = table.Column<string>(maxLength: 30, nullable: true),
                    ProvinceACFk = table.Column<int>(nullable: true),
                    InfoContactPerson = table.Column<string>(nullable: true),
                    PhoneNumberContactPerson = table.Column<string>(nullable: true),
                    IsUnionMember = table.Column<bool>(nullable: true),
                    TaxIDNumber = table.Column<string>(maxLength: 15, nullable: true),
                    BankAccountNumber = table.Column<string>(maxLength: 20, nullable: true),
                    BankAccountName = table.Column<string>(maxLength: 50, nullable: true),
                    BankName = table.Column<string>(maxLength: 100, nullable: true),
                    IDAccount = table.Column<string>(maxLength: 10, nullable: true),
                    FamilyCircumstances = table.Column<int>(nullable: true),
                    NOChildren = table.Column<int>(nullable: true),
                    NumberOfProfile = table.Column<string>(nullable: true),
                    NumberOfContract = table.Column<string>(nullable: true),
                    LaborContractType = table.Column<int>(nullable: true),
                    LaborContract = table.Column<string>(nullable: true),
                    Department_FK = table.Column<int>(nullable: true),
                    WorkingGroupFk = table.Column<int>(nullable: true),
                    Position_FK = table.Column<int>(nullable: true),
                    SignContractDate = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    TimeExpireContract = table.Column<DateTime>(nullable: true),
                    TimeExpireProbation = table.Column<DateTime>(nullable: true),
                    LayOffDate = table.Column<DateTime>(nullable: true),
                    InfoSaveFile = table.Column<string>(nullable: true),
                    LiteracyFk = table.Column<int>(nullable: true),
                    NumberOfDependents = table.Column<int>(nullable: true),
                    IDSocialInsurance = table.Column<string>(maxLength: 15, nullable: true),
                    SalarySocialInsurance = table.Column<decimal>(nullable: false),
                    SocialInsuranceDate = table.Column<DateTime>(nullable: true),
                    Salary = table.Column<decimal>(nullable: false),
                    TravelAllowance = table.Column<decimal>(nullable: true),
                    PositionAllowance = table.Column<decimal>(nullable: true),
                    SeniorityAllowances = table.Column<decimal>(nullable: true),
                    LastupdatedBy_FK = table.Column<Guid>(nullable: true),
                    User_FK = table.Column<Guid>(nullable: true),
                    Note = table.Column<string>(maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    HandoverToFKNavigationKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Employee", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.HP_Employee_dbo.HP_Department_Department_FK",
                        column: x => x.Department_FK,
                        principalTable: "HP_Department",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_HpEmployeeTypes_EmployeeTypeFK",
                        column: x => x.EmployeeTypeFK,
                        principalTable: "HpEmployeeTypes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.User_dbo.HP_Employee_Employee_FK",
                        column: x => x.LastupdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_HP_EducationalLevel_LiteracyFk",
                        column: x => x.LiteracyFk,
                        principalTable: "HP_EducationalLevel",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.HP_Employee_dbo.GN_Nation_Nation_FK",
                        column: x => x.Nation_FK,
                        principalTable: "GN_Nation",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_HP_Nationality_NationalityFk",
                        column: x => x.NationalityFk,
                        principalTable: "HP_Nationality",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.HP_Employee_dbo.HP_Position_Position_FK",
                        column: x => x.Position_FK,
                        principalTable: "HP_Position",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_GN_City_ProvinceACFk",
                        column: x => x.ProvinceACFk,
                        principalTable: "GN_City",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_GN_City_ProvincePRFk",
                        column: x => x.ProvincePRFk,
                        principalTable: "GN_City",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.HP_Employee_dbo.GN_Religion_Religion_FK",
                        column: x => x.Religion_FK,
                        principalTable: "GN_Religion",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_User_User_FK",
                        column: x => x.User_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_Employee_HP_WorkingGroup_WorkingGroupFk",
                        column: x => x.WorkingGroupFk,
                        principalTable: "HP_WorkingGroup",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HP_AllowedVacation",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    TimeKeepingTypeFK = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: true),
                    ToDate = table.Column<DateTime>(nullable: true),
                    NODate = table.Column<int>(nullable: false),
                    PercentSalary = table.Column<double>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    StatusFK = table.Column<int>(nullable: false),
                    HandoverToFK = table.Column<int>(nullable: false),
                    CommitBackToWork = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_AllowedVacation", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.HpAllowedVacation_dbo.HP_Employee_Employee_FK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.HpAllowedVacation_dbo.HP_Employee_HandoverTo_FK",
                        column: x => x.HandoverToFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.HpAllowedVacation_dbo.HP_HpAllowedVacationStatus_Status_FK",
                        column: x => x.StatusFK,
                        principalTable: "HP_AllowedVacationStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.HpAllowedVacation_dbo.HP_HpTimekeepingType_TimeKeepingType_FK",
                        column: x => x.TimeKeepingTypeFK,
                        principalTable: "HP_TimekeepingType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_BussinessApplication",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    CreatedByFK = table.Column<Guid>(nullable: false),
                    BussinessAddress = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: true),
                    ToDate = table.Column<DateTime>(nullable: true),
                    NODaysBussiness = table.Column<int>(nullable: false),
                    Allowance = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    StatusFK = table.Column<int>(nullable: false),
                    HasVehicles = table.Column<bool>(nullable: true),
                    HasHotel = table.Column<bool>(nullable: true),
                    HasLeterOfIntroduction = table.Column<bool>(nullable: true),
                    Others = table.Column<bool>(nullable: true),
                    OrthersContent = table.Column<string>(nullable: true),
                    LinkVehicles = table.Column<string>(nullable: true),
                    LinkHotel = table.Column<string>(nullable: true),
                    LinkLeterOfIntroduction = table.Column<string>(nullable: true),
                    LinkOthers = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    CreatedByFKNavigationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_BussinessApplication", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_BussinessApplication_User_CreatedByFKNavigationId",
                        column: x => x.CreatedByFKNavigationId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_BussinessApplication_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_ExpertiseDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    DegreeFK = table.Column<int>(nullable: true),
                    MajorFK = table.Column<int>(nullable: true),
                    TrainingSystemFK = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_ExpertiseDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_ExpertiseDetail_HP_Qualification_DegreeFK",
                        column: x => x.DegreeFK,
                        principalTable: "HP_Qualification",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_ExpertiseDetail_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HP_ExpertiseDetail_HP_Major_MajorFK",
                        column: x => x.MajorFK,
                        principalTable: "HP_Major",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_ExpertiseDetail_HP_TrainingSystem_TrainingSystemFK",
                        column: x => x.TrainingSystemFK,
                        principalTable: "HP_TrainingSystem",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HP_FamilyCircumstancesDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    RelativesName = table.Column<string>(maxLength: 35, nullable: true),
                    YearOfBirth = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: true),
                    Relationship = table.Column<string>(maxLength: 8, nullable: true),
                    Job = table.Column<string>(maxLength: 30, nullable: true),
                    Died = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_FamilyCircumstancesDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_FamilyCircumstancesDetail_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_LanguageDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    LanguageFK = table.Column<int>(nullable: true),
                    LevelFK = table.Column<int>(nullable: true),
                    Certificate = table.Column<string>(maxLength: 25, nullable: true),
                    LevelFKNavigationKeyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_LanguageDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_LanguageDetail_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HP_LanguageDetail_HP_Language_LanguageFK",
                        column: x => x.LanguageFK,
                        principalTable: "HP_Language",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HP_LanguageDetail_HP_Qualification_LevelFKNavigationKeyId",
                        column: x => x.LevelFKNavigationKeyId,
                        principalTable: "HP_Qualification",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HP_PermitDays",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    NOPermitDays = table.Column<int>(nullable: false),
                    NODaysUsed = table.Column<int>(nullable: false),
                    NODaysRemain = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_PermitDays", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_PermitDays_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_Salary",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    TotalWorkDay = table.Column<int>(nullable: false),
                    TotalAnnualLeave = table.Column<int>(nullable: false),
                    TotalPrivateNo = table.Column<int>(nullable: false),
                    TotalSalaryDays = table.Column<int>(nullable: false),
                    NOHoursOvertime = table.Column<double>(nullable: true),
                    TotalWorkDayOT = table.Column<double>(nullable: true),
                    SocialInsurance = table.Column<decimal>(nullable: true),
                    HealthInsurance = table.Column<decimal>(nullable: true),
                    UnemploymentInsurance = table.Column<decimal>(nullable: true),
                    UnionFee = table.Column<decimal>(nullable: true),
                    Allowance = table.Column<decimal>(nullable: true),
                    DeductionAmountDayOff = table.Column<decimal>(nullable: true),
                    OvertimeSalary = table.Column<decimal>(nullable: true),
                    TotalSalary = table.Column<decimal>(nullable: true),
                    RealSalary = table.Column<decimal>(nullable: true),
                    FamilyAllowances = table.Column<decimal>(nullable: true),
                    TaxableIncome = table.Column<decimal>(nullable: true),
                    IncomeTax = table.Column<decimal>(nullable: true),
                    Remain = table.Column<decimal>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_Salary", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_Salary_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_SalaryDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    FluctuationsDate = table.Column<DateTime>(nullable: true),
                    SalaryBeforeInscrease = table.Column<decimal>(nullable: true),
                    PercentInscrease = table.Column<double>(nullable: true),
                    AmountInscrease = table.Column<decimal>(nullable: true),
                    SalaryAfterInscrease = table.Column<decimal>(nullable: true),
                    Note = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_SalaryDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_SalaryDetail_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HP_ScoreEmployee",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    HoursIn = table.Column<string>(nullable: true),
                    HoursOut = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    NOWorkHours = table.Column<double>(nullable: true),
                    Work = table.Column<double>(nullable: true),
                    OverTime = table.Column<double>(nullable: true),
                    WorkOT = table.Column<double>(nullable: true),
                    TimeKeepingTypeFK = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_ScoreEmployee", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_ScoreEmployee_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HP_ScoreEmployee_HP_TimekeepingType_TimeKeepingTypeFK",
                        column: x => x.TimeKeepingTypeFK,
                        principalTable: "HP_TimekeepingType",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HP_WorkingProcessDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeFK = table.Column<int>(nullable: false),
                    MobilizeDate = table.Column<DateTime>(nullable: true),
                    NumberDesignation = table.Column<string>(maxLength: 15, nullable: true),
                    OldWorkPlace = table.Column<string>(maxLength: 25, nullable: true),
                    OldPosition = table.Column<string>(maxLength: 20, nullable: true),
                    CurrentWorkPlace = table.Column<string>(maxLength: 25, nullable: true),
                    CurrentPosition = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HP_WorkingProcessDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_HP_WorkingProcessDetail_HP_Employee_EmployeeFK",
                        column: x => x.EmployeeFK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO_PurchaseOrder",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SoFk = table.Column<int>(nullable: false),
                    ProjectName = table.Column<string>(maxLength: 700, nullable: true),
                    PO_ID = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    PO_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    PONumber = table.Column<string>(maxLength: 25, nullable: true),
                    POStatus_FK = table.Column<int>(unicode: false, maxLength: 25, nullable: false),
                    Vendor_FK = table.Column<int>(nullable: true),
                    Buyer_FK = table.Column<int>(nullable: false),
                    LastUpdateBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Incoterm_ID = table.Column<string>(maxLength: 255, nullable: true),
                    ShippingAddress = table.Column<string>(maxLength: 500, nullable: true),
                    PaymentTerm = table.Column<string>(maxLength: 2000, nullable: true),
                    FinishTime = table.Column<string>(maxLength: 255, nullable: true),
                    WarrantyPeriod = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    PO_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TaxRate = table.Column<double>(nullable: true),
                    Tax_Amount = table.Column<decimal>(nullable: false),
                    Total_Amount = table.Column<decimal>(nullable: false),
                    Tax_AmountB2 = table.Column<decimal>(nullable: false),
                    Total_AmountB2 = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_PurchaseOrder", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_PurchaseOrder_dbo.HP_Employee_Buyer_FK",
                        column: x => x.Buyer_FK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PO_PurchaseOrder_User_LastUpdateBy_FK",
                        column: x => x.LastUpdateBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.PO_PurchaseOrder_dbo.PO_OrderStatus_POStatus_FK",
                        column: x => x.POStatus_FK,
                        principalTable: "PO_OrderStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.PO_PurchaseOrder_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_RecReport",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RRNo = table.Column<string>(maxLength: 15, nullable: true),
                    RRDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy_FK = table.Column<Guid>(nullable: true),
                    PO_FK = table.Column<int>(nullable: true),
                    PO_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Vendor_FK = table.Column<int>(nullable: true),
                    Invoice_No = table.Column<int>(nullable: true),
                    Invoice_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Receiver_FK = table.Column<int>(nullable: false),
                    BillOf_Loading_No = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    PO_Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Inactive = table.Column<bool>(nullable: true),
                    Warehoused = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_RecReport", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_RecReport_dbo.User_CreatedBy_FK",
                        column: x => x.CreatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.PO_RecReport_dbo.HP_Employee_Receiver_FK",
                        column: x => x.Receiver_FK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.PO_RecReport_dbo.PO_Vendor_Vendor_FK",
                        column: x => x.Vendor_FK,
                        principalTable: "PO_Vendor",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SO_SalesOrder",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SOStatus_FK = table.Column<int>(nullable: true),
                    CustomerFK = table.Column<int>(nullable: true),
                    CustomerPONumber = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    CustomerPODate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NameShipTo = table.Column<string>(nullable: true),
                    AddressNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Street = table.Column<string>(maxLength: 50, nullable: true),
                    Ward = table.Column<string>(maxLength: 20, nullable: true),
                    District = table.Column<string>(maxLength: 20, nullable: true),
                    City = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    ShipViaFK = table.Column<int>(nullable: true),
                    ActualShipDate = table.Column<DateTime>(nullable: true),
                    ExpectedShipDate = table.Column<DateTime>(nullable: true),
                    ShipperFK = table.Column<int>(nullable: true),
                    PaymentTerm_FK = table.Column<int>(nullable: true),
                    PaymentType_FK = table.Column<int>(nullable: true),
                    TaxRate = table.Column<double>(nullable: true),
                    DiscountPercent = table.Column<double>(nullable: true),
                    DiscountAmt = table.Column<decimal>(nullable: false),
                    TaxAmount = table.Column<decimal>(nullable: false),
                    Freight = table.Column<decimal>(nullable: false),
                    Subtotal = table.Column<decimal>(nullable: false),
                    TotalOrder = table.Column<decimal>(nullable: false),
                    SalesPerson_FK = table.Column<int>(nullable: true),
                    CreatedBy_FK = table.Column<Guid>(nullable: true),
                    LastUpdatedBy_FK = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_SalesOrder", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_SalesOrder_dbo.User_CreatedBy_FK",
                        column: x => x.CreatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_SalesOrder_SO_Customer_CustomerFK",
                        column: x => x.CustomerFK,
                        principalTable: "SO_Customer",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_SalesOrder_dbo.User_LastUpdatedBy_FK",
                        column: x => x.LastUpdatedBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_SalesOrder_GN_PaymentTerms_PaymentTerm_FK",
                        column: x => x.PaymentTerm_FK,
                        principalTable: "GN_PaymentTerms",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_SalesOrder_GN_PaymentTypes_PaymentType_FK",
                        column: x => x.PaymentType_FK,
                        principalTable: "GN_PaymentTypes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_SalesOrder_dbo.HP_Employee_SalesPerson_FK",
                        column: x => x.SalesPerson_FK,
                        principalTable: "HP_Employee",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_SalesOrder_dbo.SO_OrderStatus_SOStatus_FK",
                        column: x => x.SOStatus_FK,
                        principalTable: "SO_OrderStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_SalesOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PO_PurchaseOrderDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PO_FK = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 25, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Unit_FK = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(maxLength: 20, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Qty_Ordered = table.Column<double>(nullable: false),
                    SubTotalOrder = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SubTotalOrderB2 = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_PurchaseOrderDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_PurchaseOrderDetail_dbo.PO_PurchaseOrder_PO_FK",
                        column: x => x.PO_FK,
                        principalTable: "PO_PurchaseOrder",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PO_RecOrderDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RRNo_FK = table.Column<int>(nullable: false),
                    Inventory_FK = table.Column<int>(nullable: false),
                    UnitFK = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Qty_Ordered = table.Column<double>(nullable: true),
                    Qty_Received = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PO_RecOrderDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.PO_RecOrderDetail_dbo.PO_RecReport_RRNo_FK",
                        column: x => x.RRNo_FK,
                        principalTable: "PO_RecReport",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SO_Acceptance",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(unicode: false, maxLength: 15, nullable: true),
                    Acceptance_ID = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    PaymentPeriod = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    SO_FK = table.Column<int>(nullable: true),
                    SOStatus_FK = table.Column<int>(nullable: true),
                    Engineer_FK = table.Column<Guid>(nullable: true),
                    AcceptanceStatus_FK = table.Column<int>(nullable: true),
                    Payment_ID = table.Column<string>(maxLength: 15, nullable: true),
                    Payment_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastUpdatePayment_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatePaymentBy_FK = table.Column<Guid>(nullable: true),
                    PaymentStatus_FK = table.Column<int>(nullable: true),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_Acceptance", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.SO_AcceptanceStatus_AcceptanceStatus_FK",
                        column: x => x.AcceptanceStatus_FK,
                        principalTable: "SO_AcceptanceStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.User_CreatePaymentBy_FK",
                        column: x => x.CreatePaymentBy_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.User_Engineer_FK",
                        column: x => x.Engineer_FK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.SO_PaymentStatus_PaymentStatus_FK",
                        column: x => x.PaymentStatus_FK,
                        principalTable: "SO_PaymentStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.SO_SalesOrder_SO_FK",
                        column: x => x.SO_FK,
                        principalTable: "SO_SalesOrder",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_Acceptance_dbo.SO_OrderStatus_SOStatus_FK",
                        column: x => x.SOStatus_FK,
                        principalTable: "SO_OrderStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SO_OrderDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SODetailStatus_FK = table.Column<int>(nullable: true),
                    SO_FK = table.Column<int>(nullable: true),
                    InventoryFK = table.Column<int>(nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Qty_On_Hand = table.Column<int>(nullable: true),
                    QtyOrdered = table.Column<int>(nullable: true),
                    QtyOrderedNow = table.Column<int>(nullable: true),
                    QtyBackOrdered = table.Column<int>(nullable: true),
                    QtyPicked = table.Column<int>(nullable: true),
                    Subtotal = table.Column<decimal>(nullable: false),
                    DiscountPercent = table.Column<double>(nullable: true),
                    Discount = table.Column<decimal>(nullable: false),
                    TaxRate = table.Column<double>(nullable: true),
                    Tax = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_OrderDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_SO_OrderDetail_IC_Inventory_InventoryFK",
                        column: x => x.InventoryFK,
                        principalTable: "IC_Inventory",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_OrderDetail_dbo.SO_SalesOrder_SO_FK",
                        column: x => x.SO_FK,
                        principalTable: "SO_SalesOrder",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_OrderDetail_dbo.SO_OrderDetailStatus_SODetailStatus_FK",
                        column: x => x.SODetailStatus_FK,
                        principalTable: "SO_OrderDetailStatus",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_OrderDetail_dbo.IC_Unit_UnitFK",
                        column: x => x.UnitFK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SO_PickTicket",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PickClerkNo = table.Column<Guid>(nullable: true),
                    SO_FK = table.Column<int>(nullable: true),
                    ExpectedShipDate = table.Column<DateTime>(nullable: true),
                    ScheduledDate = table.Column<DateTime>(nullable: true),
                    ShipDate = table.Column<DateTime>(nullable: true),
                    ShippingClerkNo = table.Column<Guid>(nullable: true),
                    Freight = table.Column<decimal>(nullable: false),
                    Qty_Mismatched = table.Column<bool>(nullable: true),
                    ShipNo = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    StatusShip = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_PickTicket", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_SO_PickTicket_User_PickClerkNo",
                        column: x => x.PickClerkNo,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_PickTicket_SO_SalesOrder_SO_FK",
                        column: x => x.SO_FK,
                        principalTable: "SO_SalesOrder",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SO_PickTicket_User_ShippingClerkNo",
                        column: x => x.ShippingClerkNo,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoPurchaseOrderBoms",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PoDetailFf = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 25, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(maxLength: 20, nullable: true),
                    QtyOrder = table.Column<double>(nullable: false),
                    PriceB2 = table.Column<decimal>(nullable: false),
                    TotalQtyB2 = table.Column<double>(nullable: false),
                    TotalQtyOrdered = table.Column<double>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoPurchaseOrderBoms", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_PoPurchaseOrderBoms_PO_PurchaseOrderDetail_PoDetailFf",
                        column: x => x.PoDetailFf,
                        principalTable: "PO_PurchaseOrderDetail",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SO_AcceptanceDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Acceptance_FK = table.Column<int>(nullable: false),
                    OrderDetail_FK = table.Column<int>(nullable: true),
                    Unit_Name = table.Column<string>(nullable: true),
                    Unit_FK = table.Column<int>(nullable: true),
                    QtyOrdered = table.Column<double>(nullable: true),
                    AccumulatedLastQtyAcc = table.Column<double>(nullable: true),
                    QtyAcceptanceNow = table.Column<double>(nullable: true),
                    AccumulatedNowQtyAcc = table.Column<double>(nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AccumulatedLastBillAcc = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    AccumulatedNowBillAcc = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_AcceptanceDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.SO_AcceptanceDetail_dbo.SO_Acceptance_Acceptance_FK",
                        column: x => x.Acceptance_FK,
                        principalTable: "SO_Acceptance",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.SO_AcceptanceDetail_dbo.SO_OrderDetail_OrderDetail_FK",
                        column: x => x.OrderDetail_FK,
                        principalTable: "SO_OrderDetail",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.SO_AcceptanceDetail_dbo.IC_Unit_Unit_FK",
                        column: x => x.Unit_FK,
                        principalTable: "IC_Unit",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SO_PickTicketDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Pick_No = table.Column<int>(nullable: true),
                    InventoryFK = table.Column<int>(nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    QtyOrderedNow = table.Column<int>(nullable: true),
                    QtyPicked = table.Column<int>(nullable: true),
                    QtyPickedShipped = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SO_PickTicketDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_SO_PickTicketDetail_SO_PickTicket_Pick_No",
                        column: x => x.Pick_No,
                        principalTable: "SO_PickTicket",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoPurchaseOrderBomDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PurchaseOrderBomFK = table.Column<int>(nullable: false),
                    BomDetaiFK = table.Column<string>(maxLength: 25, nullable: true),
                    NameBomDetail = table.Column<string>(maxLength: 500, nullable: true),
                    UnitFK = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(maxLength: 20, nullable: true),
                    QtyB2 = table.Column<double>(nullable: false),
                    QtyOrder = table.Column<double>(nullable: false),
                    PriceB2 = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoPurchaseOrderBomDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_PoPurchaseOrderBomDetails_PoPurchaseOrderBoms_PurchaseOrderB~",
                        column: x => x.PurchaseOrderBomFK,
                        principalTable: "PoPurchaseOrderBoms",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectId",
                table: "AR_AccountsReceivable",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorized_By",
                table: "AR_AccountsReceivableAdjustments",
                column: "Authorized_By");

            migrationBuilder.CreateIndex(
                name: "IX_Tran_Type",
                table: "AR_AccountsReceivableAdjustments",
                column: "Tran_Type");

            migrationBuilder.CreateIndex(
                name: "IX_ARInvoiceStatus_FK",
                table: "AR_Invoice",
                column: "ARInvoiceStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_FK",
                table: "AR_Invoice",
                column: "Customer_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerm_FK",
                table: "AR_Invoice",
                column: "PaymentTerm_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Salesperson_No",
                table: "AR_Invoice",
                column: "Salesperson_No");

            migrationBuilder.CreateIndex(
                name: "IX_SO_ID",
                table: "AR_Invoice",
                column: "SO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_FK",
                table: "AR_InvoiceDetail",
                column: "Inventory_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_FK",
                table: "AR_InvoiceDetail",
                column: "Invoice_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_FK",
                table: "AR_InvoiceDetail",
                column: "Unit_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Project_FK",
                table: "AR_PeriodPayment",
                column: "Project_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodPayment_FK",
                table: "AR_PeriodPaymentDetail",
                column: "PeriodPayment_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptanceDetails_Acceptance_FK",
                table: "BoqAcceptanceDetails",
                column: "Acceptance_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptanceDetails_OrderDetail_FK",
                table: "BoqAcceptanceDetails",
                column: "OrderDetail_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptanceDetails_Unit_FK",
                table: "BoqAcceptanceDetails",
                column: "Unit_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptances_AcceptanceStatus_FK",
                table: "BoqAcceptances",
                column: "AcceptanceStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptances_CreatePaymentBy_FK",
                table: "BoqAcceptances",
                column: "CreatePaymentBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptances_Engineer_FK",
                table: "BoqAcceptances",
                column: "Engineer_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqAcceptances_SO_FK",
                table: "BoqAcceptances",
                column: "SO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetailBoms_OrderDetailFK",
                table: "BoqOrderDetailBoms",
                column: "OrderDetailFK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetailBoms_Product_Type",
                table: "BoqOrderDetailBoms",
                column: "Product_Type");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetailBoms_UnitFK",
                table: "BoqOrderDetailBoms",
                column: "UnitFK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetails_IcUnitKeyId",
                table: "BoqOrderDetails",
                column: "IcUnitKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetails_SO_FK",
                table: "BoqOrderDetails",
                column: "SO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqOrderDetails_UnitFK",
                table: "BoqOrderDetails",
                column: "UnitFK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_CreatedBy_FK",
                table: "BoqSalesOrders",
                column: "CreatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_LastUpdatedBy_FK",
                table: "BoqSalesOrders",
                column: "LastUpdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_LastUpdatedBy_FKBOQ2",
                table: "BoqSalesOrders",
                column: "LastUpdatedBy_FKBOQ2");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_ProjectFK",
                table: "BoqSalesOrders",
                column: "ProjectFK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_SOStatus_FK",
                table: "BoqSalesOrders",
                column: "SOStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSalesOrders_SalesPerson_FK",
                table: "BoqSalesOrders",
                column: "SalesPerson_FK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSyntheticMaterials_Product_Type",
                table: "BoqSyntheticMaterials",
                column: "Product_Type");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSyntheticMaterials_SaleOrderFK",
                table: "BoqSyntheticMaterials",
                column: "SaleOrderFK");

            migrationBuilder.CreateIndex(
                name: "IX_BoqSyntheticMaterials_UnitFK",
                table: "BoqSyntheticMaterials",
                column: "UnitFK");

            migrationBuilder.CreateIndex(
                name: "IX_CreatedBy_FK",
                table: "CR_CashReceipts",
                column: "CreatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdatedBy_FK",
                table: "CR_CashReceipts",
                column: "LastUpdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_FK",
                table: "CR_CashReceiptsAccountDetail",
                column: "Receipt_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_FK",
                table: "CR_CashReceiptsDetail",
                column: "Receipt_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_No",
                table: "CR_RemittanceAdvice",
                column: "Invoice_No");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFK",
                table: "Document",
                column: "ProjectFK");

            migrationBuilder.CreateIndex(
                name: "IX_Account_FK",
                table: "GL_GeneralLedger",
                column: "Account_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodId",
                table: "GL_GeneralLedger",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_FK",
                table: "GN_CustomerLiabilities",
                column: "Project_FK");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdateBy_FK",
                table: "GN_Product",
                column: "LastUpdateBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Type",
                table: "GN_Product",
                column: "ProductTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Unit",
                table: "GN_Product",
                column: "Product_Unit");

            migrationBuilder.CreateIndex(
                name: "IX_GnProduct_Attribute_Values_productAttributeFk",
                table: "GnProduct_Attribute_Values",
                column: "productAttributeFk");

            migrationBuilder.CreateIndex(
                name: "IX_GnProduct_Links_Linked_product_id",
                table: "GnProduct_Links",
                column: "Linked_product_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GnProduct_Links_Product_id",
                table: "GnProduct_Links",
                column: "Product_id");

            migrationBuilder.CreateIndex(
                name: "IX_GnProduct_ProductAttributes_Product_AttributeFk",
                table: "GnProduct_ProductAttributes",
                column: "Product_AttributeFk");

            migrationBuilder.CreateIndex(
                name: "IX_GnProduct_ProductAttributes_product_fk",
                table: "GnProduct_ProductAttributes",
                column: "product_fk");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFK",
                table: "GNProject",
                column: "CustomerFK");

            migrationBuilder.CreateIndex(
                name: "IX_GNProject_LastUpdatedBy_FK",
                table: "GNProject",
                column: "LastUpdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Region_FK",
                table: "GNProject",
                column: "Region_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Status",
                table: "GNProject",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFK",
                table: "GNProjectDetails",
                column: "ProjectFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_AllowedVacation_EmployeeFK",
                table: "HP_AllowedVacation",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_AllowedVacation_HandoverToFK",
                table: "HP_AllowedVacation",
                column: "HandoverToFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_AllowedVacation_StatusFK",
                table: "HP_AllowedVacation",
                column: "StatusFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_AllowedVacation_TimeKeepingTypeFK",
                table: "HP_AllowedVacation",
                column: "TimeKeepingTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_BussinessApplication_CreatedByFKNavigationId",
                table: "HP_BussinessApplication",
                column: "CreatedByFKNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_HP_BussinessApplication_EmployeeFK",
                table: "HP_BussinessApplication",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_Department_FK",
                table: "HP_Employee",
                column: "Department_FK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_EmployeeTypeFK",
                table: "HP_Employee",
                column: "EmployeeTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_HandoverToFKNavigationKeyId",
                table: "HP_Employee",
                column: "HandoverToFKNavigationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_LastupdatedBy_FK",
                table: "HP_Employee",
                column: "LastupdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_LiteracyFk",
                table: "HP_Employee",
                column: "LiteracyFk");

            migrationBuilder.CreateIndex(
                name: "IX_Nation_FK",
                table: "HP_Employee",
                column: "Nation_FK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_NationalityFk",
                table: "HP_Employee",
                column: "NationalityFk");

            migrationBuilder.CreateIndex(
                name: "IX_Position_FK",
                table: "HP_Employee",
                column: "Position_FK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_ProvinceACFk",
                table: "HP_Employee",
                column: "ProvinceACFk");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_ProvincePRFk",
                table: "HP_Employee",
                column: "ProvincePRFk");

            migrationBuilder.CreateIndex(
                name: "IX_Religion_FK",
                table: "HP_Employee",
                column: "Religion_FK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_User_FK",
                table: "HP_Employee",
                column: "User_FK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HP_Employee_WorkingGroupFk",
                table: "HP_Employee",
                column: "WorkingGroupFk");

            migrationBuilder.CreateIndex(
                name: "IX_HP_EmployeeLog_ChangeDepartmentFK",
                table: "HP_EmployeeLog",
                column: "ChangeDepartmentFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_EmployeeLog_ChangeWorkingGroupFK",
                table: "HP_EmployeeLog",
                column: "ChangeWorkingGroupFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ExpertiseDetail_DegreeFK",
                table: "HP_ExpertiseDetail",
                column: "DegreeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ExpertiseDetail_EmployeeFK",
                table: "HP_ExpertiseDetail",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ExpertiseDetail_MajorFK",
                table: "HP_ExpertiseDetail",
                column: "MajorFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ExpertiseDetail_TrainingSystemFK",
                table: "HP_ExpertiseDetail",
                column: "TrainingSystemFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_FamilyCircumstancesDetail_EmployeeFK",
                table: "HP_FamilyCircumstancesDetail",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_LanguageDetail_EmployeeFK",
                table: "HP_LanguageDetail",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_LanguageDetail_LanguageFK",
                table: "HP_LanguageDetail",
                column: "LanguageFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_LanguageDetail_LevelFKNavigationKeyId",
                table: "HP_LanguageDetail",
                column: "LevelFKNavigationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_HP_PermitDays_EmployeeFK",
                table: "HP_PermitDays",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Rank_QuotaFK",
                table: "HP_Rank",
                column: "QuotaFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_Salary_EmployeeFK",
                table: "HP_Salary",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_SalaryDetail_EmployeeFK",
                table: "HP_SalaryDetail",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ScoreEmployee_EmployeeFK",
                table: "HP_ScoreEmployee",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_ScoreEmployee_TimeKeepingTypeFK",
                table: "HP_ScoreEmployee",
                column: "TimeKeepingTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_HP_WorkingProcessDetail_EmployeeFK",
                table: "HP_WorkingProcessDetail",
                column: "EmployeeFK");

            migrationBuilder.CreateIndex(
                name: "IX_IC_Inventory_IcCategoryKeyId",
                table: "IC_Inventory",
                column: "IcCategoryKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_IC_Inventory_IcUnitKeyId",
                table: "IC_Inventory",
                column: "IcUnitKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdateBy_FK",
                table: "IC_Inventory",
                column: "LastUpdateBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FK",
                table: "IC_Inventory",
                column: "Product_FK");

            migrationBuilder.CreateIndex(
                name: "IX_VendorNumber_FK",
                table: "IC_Inventory",
                column: "VendorNumber_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_FK",
                table: "IC_Inventory",
                column: "Warehouse_FK");

            migrationBuilder.CreateIndex(
                name: "IX_IC_InventoryCustomerPrice_CustomerFk",
                table: "IC_InventoryCustomerPrice",
                column: "CustomerFk");

            migrationBuilder.CreateIndex(
                name: "IX_IC_InventoryCustomerPrice_InventoryFk",
                table: "IC_InventoryCustomerPrice",
                column: "InventoryFk");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdatedBy",
                table: "IC_Unit",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitCategory_FK",
                table: "IC_Unit",
                column: "UnitCategory_FK");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdatedBy",
                table: "IC_UnitCategory",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBOM_FK",
                table: "MRP_BOM",
                column: "ProductBOM_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Product_FK",
                table: "MRP_BOM",
                column: "Product_FK");

            migrationBuilder.CreateIndex(
                name: "IX_MRP_BOM_UserId",
                table: "MRP_BOM",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_AccountPayable",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Authorized_By",
                table: "PO_AccountsPayableAdjustments",
                column: "Authorized_By");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_AccountsPayableAdjustments",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_APInvoices",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_FK",
                table: "PO_APInvoicesDetail",
                column: "Invoice_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_CashDisbursement",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PO_CashDisb_No_FK",
                table: "PO_CashDisbursementDetail",
                column: "PO_CashDisb_No_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Buyer_FK",
                table: "PO_PurchaseOrder",
                column: "Buyer_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PO_PurchaseOrder_LastUpdateBy_FK",
                table: "PO_PurchaseOrder",
                column: "LastUpdateBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_POStatus_FK",
                table: "PO_PurchaseOrder",
                column: "POStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_PurchaseOrder",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PO_FK",
                table: "PO_PurchaseOrderDetail",
                column: "PO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_RRNo_FK",
                table: "PO_RecOrderDetail",
                column: "RRNo_FK");

            migrationBuilder.CreateIndex(
                name: "IX_CreatedBy_FK",
                table: "PO_RecReport",
                column: "CreatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Receiver_FK",
                table: "PO_RecReport",
                column: "Receiver_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_FK",
                table: "PO_RecReport",
                column: "Vendor_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Vendor_LastUpdatedFk",
                table: "PO_Vendor",
                column: "LastUpdatedFk");

            migrationBuilder.CreateIndex(
                name: "IX_PO_Vendor_UserFk",
                table: "PO_Vendor",
                column: "UserFk",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PoPurchaseOrderBomDetails_PurchaseOrderBomFK",
                table: "PoPurchaseOrderBomDetails",
                column: "PurchaseOrderBomFK");

            migrationBuilder.CreateIndex(
                name: "IX_PoPurchaseOrderBoms_PoDetailFf",
                table: "PoPurchaseOrderBoms",
                column: "PoDetailFf");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptanceStatus_FK",
                table: "SO_Acceptance",
                column: "AcceptanceStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_CreatePaymentBy_FK",
                table: "SO_Acceptance",
                column: "CreatePaymentBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Engineer_FK",
                table: "SO_Acceptance",
                column: "Engineer_FK");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatus_FK",
                table: "SO_Acceptance",
                column: "PaymentStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_FK",
                table: "SO_Acceptance",
                column: "SO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SOStatus_FK",
                table: "SO_Acceptance",
                column: "SOStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Acceptance_FK",
                table: "SO_AcceptanceDetail",
                column: "Acceptance_FK");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_FK",
                table: "SO_AcceptanceDetail",
                column: "OrderDetail_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_FK",
                table: "SO_AcceptanceDetail",
                column: "Unit_FK");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdatedBy_FK",
                table: "SO_Customer",
                column: "LastUpdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_Customer_UserFk",
                table: "SO_Customer",
                column: "UserFk",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Acceptance_FK",
                table: "SO_Notification",
                column: "Acceptance_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_FK",
                table: "SO_Notification",
                column: "Customer_FK");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_No",
                table: "SO_Notification",
                column: "Invoice_No");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatus_FK",
                table: "SO_Notification",
                column: "PaymentStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_ID",
                table: "SO_Notification",
                column: "SO_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_FK",
                table: "SO_NotificationGeneralDetail",
                column: "Notification_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_OrderDetail_InventoryFK",
                table: "SO_OrderDetail",
                column: "InventoryFK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_FK",
                table: "SO_OrderDetail",
                column: "SO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SODetailStatus_FK",
                table: "SO_OrderDetail",
                column: "SODetailStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_UnitFK",
                table: "SO_OrderDetail",
                column: "UnitFK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_PickTicket_PickClerkNo",
                table: "SO_PickTicket",
                column: "PickClerkNo");

            migrationBuilder.CreateIndex(
                name: "IX_SO_PickTicket_SO_FK",
                table: "SO_PickTicket",
                column: "SO_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_PickTicket_ShippingClerkNo",
                table: "SO_PickTicket",
                column: "ShippingClerkNo");

            migrationBuilder.CreateIndex(
                name: "IX_SO_PickTicketDetail_Pick_No",
                table: "SO_PickTicketDetail",
                column: "Pick_No");

            migrationBuilder.CreateIndex(
                name: "IX_CreatedBy_FK",
                table: "SO_SalesOrder",
                column: "CreatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_SalesOrder_CustomerFK",
                table: "SO_SalesOrder",
                column: "CustomerFK");

            migrationBuilder.CreateIndex(
                name: "IX_LastUpdatedBy_FK",
                table: "SO_SalesOrder",
                column: "LastUpdatedBy_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_SalesOrder_PaymentTerm_FK",
                table: "SO_SalesOrder",
                column: "PaymentTerm_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_SalesOrder_PaymentType_FK",
                table: "SO_SalesOrder",
                column: "PaymentType_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPerson_FK",
                table: "SO_SalesOrder",
                column: "SalesPerson_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SOStatus_FK",
                table: "SO_SalesOrder",
                column: "SOStatus_FK");

            migrationBuilder.CreateIndex(
                name: "IX_SO_SalesOrder_UserId",
                table: "SO_SalesOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupID",
                table: "tblMenuGroup",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuId",
                table: "tblMenuGroup",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_User_GnCityKeyId",
                table: "User",
                column: "GnCityKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupID",
                table: "UserGroup",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserID",
                table: "UserGroup",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectId",
                table: "UserProject",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserID",
                table: "UserProject",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.AR_InvoiceDetail_dbo.AR_Invoice_Invoice_FK",
                table: "AR_InvoiceDetail",
                column: "Invoice_FK",
                principalTable: "AR_Invoice",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.CR_RemittanceAdvice_dbo.AR_Invoice_Invoice_No",
                table: "CR_RemittanceAdvice",
                column: "Invoice_No",
                principalTable: "AR_Invoice",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.SO_Notification_dbo.SO_SalesOrder_SO_ID",
                table: "SO_Notification",
                column: "SO_ID",
                principalTable: "SO_SalesOrder",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.SO_Notification_dbo.AR_Invoice_Invoice_No",
                table: "SO_Notification",
                column: "Invoice_No",
                principalTable: "AR_Invoice",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.SO_Notification_dbo.SO_Acceptance_Acceptance_FK",
                table: "SO_Notification",
                column: "Acceptance_FK",
                principalTable: "SO_Acceptance",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AR_Invoice_SO_SalesOrder_SO_ID",
                table: "AR_Invoice",
                column: "SO_ID",
                principalTable: "SO_SalesOrder",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoqAcceptanceDetails_BoqAcceptances_Acceptance_FK",
                table: "BoqAcceptanceDetails",
                column: "Acceptance_FK",
                principalTable: "BoqAcceptances",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoqAcceptanceDetails_BoqOrderDetails_OrderDetail_FK",
                table: "BoqAcceptanceDetails",
                column: "OrderDetail_FK",
                principalTable: "BoqOrderDetails",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BoqAcceptances_BoqSalesOrders_SO_FK",
                table: "BoqAcceptances",
                column: "SO_FK",
                principalTable: "BoqSalesOrders",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoqOrderDetailBoms_BoqOrderDetails_OrderDetailFK",
                table: "BoqOrderDetailBoms",
                column: "OrderDetailFK",
                principalTable: "BoqOrderDetails",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BoqSalesOrders_HP_Employee_SalesPerson_FK",
                table: "BoqSalesOrders",
                column: "SalesPerson_FK",
                principalTable: "HP_Employee",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HP_Employee_HP_AllowedVacation_HandoverToFKNavigationKeyId",
                table: "HP_Employee",
                column: "HandoverToFKNavigationKeyId",
                principalTable: "HP_AllowedVacation",
                principalColumn: "KeyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.User_dbo.HP_Employee_Employee_FK",
                table: "HP_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_HP_Employee_User_User_FK",
                table: "HP_Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_dbo.HpAllowedVacation_dbo.HP_Employee_Employee_FK",
                table: "HP_AllowedVacation");

            migrationBuilder.DropForeignKey(
                name: "FK_dbo.HpAllowedVacation_dbo.HP_Employee_HandoverTo_FK",
                table: "HP_AllowedVacation");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "AR_AccountsReceivable");

            migrationBuilder.DropTable(
                name: "AR_AccountsReceivableAdjustments");

            migrationBuilder.DropTable(
                name: "AR_GeneralJournal");

            migrationBuilder.DropTable(
                name: "AR_InvoiceDetail");

            migrationBuilder.DropTable(
                name: "AR_PeriodPaymentDetail");

            migrationBuilder.DropTable(
                name: "AR_SalesEvent");

            migrationBuilder.DropTable(
                name: "BoqAcceptanceDetails");

            migrationBuilder.DropTable(
                name: "BoqOrderDetailBoms");

            migrationBuilder.DropTable(
                name: "BoqSyntheticMaterials");

            migrationBuilder.DropTable(
                name: "CR_CashReceiptsAccountDetail");

            migrationBuilder.DropTable(
                name: "CR_CashReceiptsDetail");

            migrationBuilder.DropTable(
                name: "CR_RemittanceAdvice");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "EdittingForms");

            migrationBuilder.DropTable(
                name: "ErrorLogs");

            migrationBuilder.DropTable(
                name: "GL_GeneralLedger");

            migrationBuilder.DropTable(
                name: "GN_AccountDefault");

            migrationBuilder.DropTable(
                name: "GN_CustomerLiabilities");

            migrationBuilder.DropTable(
                name: "GN_ShipVia");

            migrationBuilder.DropTable(
                name: "GnProduct_Attribute_Values");

            migrationBuilder.DropTable(
                name: "GnProduct_Links");

            migrationBuilder.DropTable(
                name: "GnProduct_ProductAttributes");

            migrationBuilder.DropTable(
                name: "GNProjectDetails");

            migrationBuilder.DropTable(
                name: "HP_BussinessApplication");

            migrationBuilder.DropTable(
                name: "HP_EmployeeLog");

            migrationBuilder.DropTable(
                name: "HP_Enterprise");

            migrationBuilder.DropTable(
                name: "HP_ExpertiseDetail");

            migrationBuilder.DropTable(
                name: "HP_FamilyCircumstancesDetail");

            migrationBuilder.DropTable(
                name: "HP_IncomeTaxList");

            migrationBuilder.DropTable(
                name: "HP_LanguageDetail");

            migrationBuilder.DropTable(
                name: "HP_PermitDays");

            migrationBuilder.DropTable(
                name: "HP_Rank");

            migrationBuilder.DropTable(
                name: "HP_Salary");

            migrationBuilder.DropTable(
                name: "HP_SalaryDetail");

            migrationBuilder.DropTable(
                name: "HP_ScoreEmployee");

            migrationBuilder.DropTable(
                name: "HP_WorkingProcessDetail");

            migrationBuilder.DropTable(
                name: "IC_InventoryCustomerPrice");

            migrationBuilder.DropTable(
                name: "MRP_BOM");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PO_AccountPayable");

            migrationBuilder.DropTable(
                name: "PO_AccountsPayableAdjustments");

            migrationBuilder.DropTable(
                name: "PO_APInvoicesDetail");

            migrationBuilder.DropTable(
                name: "PO_CashDisbursementDetail");

            migrationBuilder.DropTable(
                name: "PO_Notification");

            migrationBuilder.DropTable(
                name: "PO_RecOrderDetail");

            migrationBuilder.DropTable(
                name: "PoPurchaseOrderBomDetails");

            migrationBuilder.DropTable(
                name: "SO_AcceptanceDetail");

            migrationBuilder.DropTable(
                name: "SO_Notification");

            migrationBuilder.DropTable(
                name: "SO_NotificationGeneralDetail");

            migrationBuilder.DropTable(
                name: "SO_PickTicketDetail");

            migrationBuilder.DropTable(
                name: "SO_Report");

            migrationBuilder.DropTable(
                name: "tbl_SystemParameters");

            migrationBuilder.DropTable(
                name: "tblMenuGroup");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserProject");

            migrationBuilder.DropTable(
                name: "GN_ARTransactionType");

            migrationBuilder.DropTable(
                name: "AR_PeriodPayment");

            migrationBuilder.DropTable(
                name: "BoqAcceptances");

            migrationBuilder.DropTable(
                name: "BoqOrderDetails");

            migrationBuilder.DropTable(
                name: "CR_CashReceipts");

            migrationBuilder.DropTable(
                name: "GN_Ledger");

            migrationBuilder.DropTable(
                name: "GN_Period");

            migrationBuilder.DropTable(
                name: "GnProduct_Attributes");

            migrationBuilder.DropTable(
                name: "HP_Major");

            migrationBuilder.DropTable(
                name: "HP_TrainingSystem");

            migrationBuilder.DropTable(
                name: "HP_Language");

            migrationBuilder.DropTable(
                name: "HP_Qualification");

            migrationBuilder.DropTable(
                name: "HpQuotas");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "PO_APInvoices");

            migrationBuilder.DropTable(
                name: "PO_CashDisbursement");

            migrationBuilder.DropTable(
                name: "PO_RecReport");

            migrationBuilder.DropTable(
                name: "PoPurchaseOrderBoms");

            migrationBuilder.DropTable(
                name: "SO_OrderDetail");

            migrationBuilder.DropTable(
                name: "SO_Acceptance");

            migrationBuilder.DropTable(
                name: "AR_Invoice");

            migrationBuilder.DropTable(
                name: "SO_NotificationGeneral");

            migrationBuilder.DropTable(
                name: "SO_PickTicket");

            migrationBuilder.DropTable(
                name: "tblMenu");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "BoqAcceptanceStatuses");

            migrationBuilder.DropTable(
                name: "BoqSalesOrders");

            migrationBuilder.DropTable(
                name: "PO_PurchaseOrderDetail");

            migrationBuilder.DropTable(
                name: "IC_Inventory");

            migrationBuilder.DropTable(
                name: "SO_OrderDetailStatus");

            migrationBuilder.DropTable(
                name: "SO_AcceptanceStatus");

            migrationBuilder.DropTable(
                name: "SO_PaymentStatus");

            migrationBuilder.DropTable(
                name: "AR_InvoiceStatus");

            migrationBuilder.DropTable(
                name: "SO_SalesOrder");

            migrationBuilder.DropTable(
                name: "GNProject");

            migrationBuilder.DropTable(
                name: "BoqOrderStatuses");

            migrationBuilder.DropTable(
                name: "PO_PurchaseOrder");

            migrationBuilder.DropTable(
                name: "IC_Category");

            migrationBuilder.DropTable(
                name: "GN_Product");

            migrationBuilder.DropTable(
                name: "IC_Warehouse");

            migrationBuilder.DropTable(
                name: "GN_PaymentTerms");

            migrationBuilder.DropTable(
                name: "GN_PaymentTypes");

            migrationBuilder.DropTable(
                name: "SO_OrderStatus");

            migrationBuilder.DropTable(
                name: "SO_Customer");

            migrationBuilder.DropTable(
                name: "GN_Region");

            migrationBuilder.DropTable(
                name: "GNProjectStatus");

            migrationBuilder.DropTable(
                name: "PO_OrderStatus");

            migrationBuilder.DropTable(
                name: "PO_Vendor");

            migrationBuilder.DropTable(
                name: "GN_ProductType");

            migrationBuilder.DropTable(
                name: "IC_Unit");

            migrationBuilder.DropTable(
                name: "IC_UnitCategory");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "HP_Employee");

            migrationBuilder.DropTable(
                name: "HP_Department");

            migrationBuilder.DropTable(
                name: "HpEmployeeTypes");

            migrationBuilder.DropTable(
                name: "HP_AllowedVacation");

            migrationBuilder.DropTable(
                name: "HP_EducationalLevel");

            migrationBuilder.DropTable(
                name: "GN_Nation");

            migrationBuilder.DropTable(
                name: "HP_Nationality");

            migrationBuilder.DropTable(
                name: "HP_Position");

            migrationBuilder.DropTable(
                name: "GN_City");

            migrationBuilder.DropTable(
                name: "GN_Religion");

            migrationBuilder.DropTable(
                name: "HP_WorkingGroup");

            migrationBuilder.DropTable(
                name: "HP_AllowedVacationStatus");

            migrationBuilder.DropTable(
                name: "HP_TimekeepingType");
        }
    }
}
