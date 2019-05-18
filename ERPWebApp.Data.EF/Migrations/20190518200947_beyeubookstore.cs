using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeYeuBookstore.Data.EF.Migrations
{
    public partial class beyeubookstore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvertisementPositions",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PageUrl = table.Column<string>(nullable: true),
                    IdOfPosition = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Img = table.Column<string>(nullable: true),
                    AdvertisePrice = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementPositions", x => x.KeyId);
                });

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
                name: "BookCategorys",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookCategoryName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCategorys", x => x.KeyId);
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
                name: "SoNotification",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerFK = table.Column<int>(nullable: false),
                    SoId = table.Column<int>(nullable: false),
                    Sodate = table.Column<DateTime>(nullable: false),
                    AcceptanceFk = table.Column<int>(nullable: true),
                    AcceptanceDate = table.Column<DateTime>(nullable: true),
                    EngineerFk = table.Column<int>(nullable: true),
                    PaymentFk = table.Column<int>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    CreatePaymentByFk = table.Column<int>(nullable: true),
                    PaymentStatusFk = table.Column<int>(nullable: true),
                    InvoiceNo = table.Column<int>(nullable: true),
                    NotificationDate = table.Column<DateTime>(nullable: true),
                    NotificationLastUpdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoNotification", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "SoNotificationGeneral",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificationContent = table.Column<string>(nullable: true),
                    Referent = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoNotificationGeneral", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserTypeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.KeyId);
                });

            migrationBuilder.CreateTable(
                name: "WebMasterTypes",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WebMasterTypeName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebMasterTypes", x => x.KeyId);
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
                name: "SoNotificationGeneralDetail",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotificationFk = table.Column<int>(nullable: true),
                    UserFk = table.Column<Guid>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    NotificationFkNavigationKeyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoNotificationGeneralDetail", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_SoNotificationGeneralDetail_SoNotificationGeneral_Notificati~",
                        column: x => x.NotificationFkNavigationKeyId,
                        principalTable: "SoNotificationGeneral",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
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
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    IdNumber = table.Column<string>(nullable: true),
                    Dob = table.Column<DateTime>(nullable: true),
                    IdDate = table.Column<DateTime>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserTypeFK = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LastupdatedFk = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.Users.UserTypeFK",
                        column: x => x.UserTypeFK,
                        principalTable: "UserTypes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisers",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserFK = table.Column<Guid>(nullable: false),
                    BrandName = table.Column<string>(nullable: true),
                    UrlToBrand = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisers", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_Advertisers_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserFK = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_Customers_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserFK = table.Column<Guid>(nullable: false),
                    DirectContactName = table.Column<string>(nullable: true),
                    Hotline = table.Column<string>(nullable: true),
                    MerchantCompanyName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    ContactAddress = table.Column<string>(nullable: true),
                    BussinessRegisterId = table.Column<string>(nullable: true),
                    TaxId = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    LegalRepresentative = table.Column<string>(nullable: true),
                    MerchantBankingName = table.Column<string>(nullable: true),
                    Bank = table.Column<string>(nullable: true),
                    BankBranch = table.Column<string>(nullable: true),
                    BussinessRegisterLinkImg = table.Column<string>(nullable: true),
                    BankAccountNotificationLinkImg = table.Column<string>(nullable: true),
                    TaxRegisterLinkImg = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Scales = table.Column<int>(nullable: false),
                    EstablishDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_Merchants_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebMasters",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserFK = table.Column<Guid>(nullable: false),
                    WebMasterTypeFK = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebMasters", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_WebMasters_User_UserFK",
                        column: x => x.UserFK,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.WebMasters.WebMasterTypeFK",
                        column: x => x.WebMasterTypeFK,
                        principalTable: "WebMasterTypes",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerFK = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    DeliAddress = table.Column<string>(nullable: true),
                    DeliContactName = table.Column<string>(nullable: true),
                    DeliContactHotline = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.Invoices.CustomerFK",
                        column: x => x.CustomerFK,
                        principalTable: "Customers",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookTitle = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    BookCategoryFK = table.Column<int>(nullable: false),
                    MerchantFK = table.Column<int>(nullable: false),
                    isPaperback = table.Column<bool>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Length = table.Column<int>(nullable: true),
                    Height = table.Column<int>(nullable: true),
                    Width = table.Column<int>(nullable: true),
                    PageNumber = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Img = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    RatingNumber = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.Book.BookCategory_FK",
                        column: x => x.BookCategoryFK,
                        principalTable: "BookCategorys",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Book.MerchantFK",
                        column: x => x.MerchantFK,
                        principalTable: "Merchants",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksIns",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MerchantFK = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksIns", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.BooksIn.MerchantFK",
                        column: x => x.MerchantFK,
                        principalTable: "Merchants",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksOuts",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MerchantFK = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksOuts", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.BooksOut.MerchantFK",
                        column: x => x.MerchantFK,
                        principalTable: "Merchants",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantContracts",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContractLink = table.Column<string>(nullable: true),
                    MerchantFK = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantContracts", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.MerchantContract.MerchantFK",
                        column: x => x.MerchantFK,
                        principalTable: "Merchants",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementContents",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdvertisementPositionFK = table.Column<int>(nullable: true),
                    AdvertiserFK = table.Column<int>(nullable: false),
                    ImageLink = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UrlToAdvertisement = table.Column<string>(nullable: true),
                    CensorStatus = table.Column<int>(nullable: false),
                    CensorFK = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementContents", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.AdvertisementContent.AdvertisementPosition",
                        column: x => x.AdvertisementPositionFK,
                        principalTable: "AdvertisementPositions",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dbo.AdvertisementContent.Advertiser_AdvertiserFK_FK",
                        column: x => x.AdvertiserFK,
                        principalTable: "Advertisers",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.AdvertisementContent.WebMaster_CensorFK_FK",
                        column: x => x.CensorFK,
                        principalTable: "WebMasters",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceFK = table.Column<int>(nullable: false),
                    DeliveryStatus = table.Column<int>(nullable: false),
                    MerchantFK = table.Column<int>(nullable: false),
                    OrderPrice = table.Column<decimal>(nullable: false),
                    ShipPrice = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.Deliveries.InvoiceFK",
                        column: x => x.InvoiceFK,
                        principalTable: "Invoices",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Deliveries.MerchantFK",
                        column: x => x.MerchantFK,
                        principalTable: "Merchants",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceFK = table.Column<int>(nullable: false),
                    BookFK = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    SubTotal = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.InvoiceDetails.BookFK",
                        column: x => x.BookFK,
                        principalTable: "Books",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.InvoiceDetails.InvoiceFK",
                        column: x => x.InvoiceFK,
                        principalTable: "Invoices",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookFK = table.Column<int>(nullable: false),
                    CustomerFK = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    Comment = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.RatingDetails.BookFK",
                        column: x => x.BookFK,
                        principalTable: "Books",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.RatingDetails.CustomerFK",
                        column: x => x.CustomerFK,
                        principalTable: "Customers",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksInDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BooksInFK = table.Column<int>(nullable: false),
                    BookFK = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksInDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.BooksInDetail.BookFK",
                        column: x => x.BookFK,
                        principalTable: "Books",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.BooksInDetail.BooksInFK",
                        column: x => x.BooksInFK,
                        principalTable: "BooksIns",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksOutDetails",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BooksOutFK = table.Column<int>(nullable: false),
                    BookFK = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksOutDetails", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_dbo.BooksOutDetail.BookFK",
                        column: x => x.BookFK,
                        principalTable: "Books",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.BooksOutDetail.BooksOutFK",
                        column: x => x.BooksOutFK,
                        principalTable: "BooksOuts",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertiseContracts",
                columns: table => new
                {
                    KeyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdvertisementContentFK = table.Column<int>(nullable: true),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateFinish = table.Column<DateTime>(nullable: false),
                    Deposite = table.Column<decimal>(nullable: false),
                    ContractValue = table.Column<decimal>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertiseContracts", x => x.KeyId);
                    table.ForeignKey(
                        name: "FK_AdvertiseContracts_AdvertisementContents_AdvertisementConten~",
                        column: x => x.AdvertisementContentFK,
                        principalTable: "AdvertisementContents",
                        principalColumn: "KeyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertiseContracts_AdvertisementContentFK",
                table: "AdvertiseContracts",
                column: "AdvertisementContentFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementContents_AdvertisementPositionFK",
                table: "AdvertisementContents",
                column: "AdvertisementPositionFK");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementContents_AdvertiserFK",
                table: "AdvertisementContents",
                column: "AdvertiserFK");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementContents_CensorFK",
                table: "AdvertisementContents",
                column: "CensorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisers_UserFK",
                table: "Advertisers",
                column: "UserFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookCategoryFK",
                table: "Books",
                column: "BookCategoryFK");

            migrationBuilder.CreateIndex(
                name: "IX_Books_MerchantFK",
                table: "Books",
                column: "MerchantFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksInDetails_BookFK",
                table: "BooksInDetails",
                column: "BookFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksInDetails_BooksInFK",
                table: "BooksInDetails",
                column: "BooksInFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksIns_MerchantFK",
                table: "BooksIns",
                column: "MerchantFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksOutDetails_BookFK",
                table: "BooksOutDetails",
                column: "BookFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksOutDetails_BooksOutFK",
                table: "BooksOutDetails",
                column: "BooksOutFK");

            migrationBuilder.CreateIndex(
                name: "IX_BooksOuts_MerchantFK",
                table: "BooksOuts",
                column: "MerchantFK");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserFK",
                table: "Customers",
                column: "UserFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_InvoiceFK",
                table: "Deliveries",
                column: "InvoiceFK");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_MerchantFK",
                table: "Deliveries",
                column: "MerchantFK");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_BookFK",
                table: "InvoiceDetails",
                column: "BookFK");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceFK",
                table: "InvoiceDetails",
                column: "InvoiceFK");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerFK",
                table: "Invoices",
                column: "CustomerFK");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantContracts_MerchantFK",
                table: "MerchantContracts",
                column: "MerchantFK");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_UserFK",
                table: "Merchants",
                column: "UserFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_FunctionId",
                table: "Permissions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDetails_BookFK",
                table: "RatingDetails",
                column: "BookFK");

            migrationBuilder.CreateIndex(
                name: "IX_RatingDetails_CustomerFK",
                table: "RatingDetails",
                column: "CustomerFK");

            migrationBuilder.CreateIndex(
                name: "IX_SoNotificationGeneralDetail_NotificationFkNavigationKeyId",
                table: "SoNotificationGeneralDetail",
                column: "NotificationFkNavigationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserTypeFK",
                table: "User",
                column: "UserTypeFK");

            migrationBuilder.CreateIndex(
                name: "IX_WebMasters_UserFK",
                table: "WebMasters",
                column: "UserFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebMasters_WebMasterTypeFK",
                table: "WebMasters",
                column: "WebMasterTypeFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertiseContracts");

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
                name: "BooksInDetails");

            migrationBuilder.DropTable(
                name: "BooksOutDetails");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "MerchantContracts");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "RatingDetails");

            migrationBuilder.DropTable(
                name: "SoNotification");

            migrationBuilder.DropTable(
                name: "SoNotificationGeneralDetail");

            migrationBuilder.DropTable(
                name: "AdvertisementContents");

            migrationBuilder.DropTable(
                name: "BooksIns");

            migrationBuilder.DropTable(
                name: "BooksOuts");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "SoNotificationGeneral");

            migrationBuilder.DropTable(
                name: "AdvertisementPositions");

            migrationBuilder.DropTable(
                name: "Advertisers");

            migrationBuilder.DropTable(
                name: "WebMasters");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "BookCategorys");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "WebMasterTypes");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
