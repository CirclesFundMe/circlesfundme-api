using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CFM");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CFMAccounts",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CFMAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContributionSchemes",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SchemeType = table.Column<int>(type: "int", nullable: false),
                    ContributionPercent = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    EligibleLoanMultiple = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    ServiceCharge = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    LoanManagementFee = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    DefaultPenaltyPercent = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    EquityPercent = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    LoanTerm = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    PreLoanServiceChargePercent = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    PostLoanServiceChargePercent = table.Column<double>(type: "float(18)", precision: 18, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOtps",
                schema: "CFM",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOtps", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "CFM",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverPictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnboardingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AgreedToTerms = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AllowPushNotifications = table.Column<bool>(type: "bit", nullable: false),
                    AllowEmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CFMAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CFMAccounts_CFMAccountId",
                        column: x => x.CFMAccountId,
                        principalSchema: "CFM",
                        principalTable: "CFMAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "CFM",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "CFM",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "CFM",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "CFM",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceAvatar = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ObjectId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ObjectSlug = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ObjectIdentifier = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShouldSendEmailNotification = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationSubject = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsEmailNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    EmailNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailNotificationRetryCount = table.Column<int>(type: "int", nullable: false),
                    EmailNotificationErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShouldSendPushNotification = table.Column<bool>(type: "bit", nullable: false),
                    IsPushNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    PushNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PushNotificationRetryCount = table.Column<int>(type: "int", nullable: false),
                    PushNotificationErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ShouldSendSmsNotification = table.Column<bool>(type: "bit", nullable: false),
                    IsSmsNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotificationSentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SmsNotificationRetryCount = table.Column<int>(type: "int", nullable: false),
                    SmsNotificationErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_AppUserId",
                        column: x => x.AppUserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddresses",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddresses_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContributionSchemes",
                schema: "CFM",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContributionSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContributionAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IncomeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContributionSchemes", x => new { x.UserId, x.ContributionSchemeId });
                    table.ForeignKey(
                        name: "FK_UserContributionSchemes_ContributionSchemes_ContributionSchemeId",
                        column: x => x.ContributionSchemeId,
                        principalSchema: "CFM",
                        principalTable: "ContributionSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContributionSchemes_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserKYC",
                schema: "CFM",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BVN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsBvnVerified = table.Column<bool>(type: "bit", nullable: false),
                    NIN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsNinVerified = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserKYC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserKYC_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CFM",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "CFM",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "CFM",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "CFM",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "CFM",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "CFM",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CFMAccounts_Name",
                schema: "CFM",
                table: "CFMAccounts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionSchemes_SchemeType",
                schema: "CFM",
                table: "ContributionSchemes",
                column: "SchemeType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AppUserId",
                schema: "CFM",
                table: "Notifications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "CFM",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_UserId",
                schema: "CFM",
                table: "UserAddresses",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserContributionSchemes_ContributionSchemeId",
                schema: "CFM",
                table: "UserContributionSchemes",
                column: "ContributionSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContributionSchemes_UserId",
                schema: "CFM",
                table: "UserContributionSchemes",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UserId_DocumentType",
                schema: "CFM",
                table: "UserDocuments",
                columns: new[] { "UserId", "DocumentType" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_UserId",
                schema: "CFM",
                table: "UserKYC",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserOtps_Email",
                schema: "CFM",
                table: "UserOtps",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "CFM",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CFMAccountId",
                schema: "CFM",
                table: "Users",
                column: "CFMAccountId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "CFM",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "UserAddresses",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "UserContributionSchemes",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "UserDocuments",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "UserKYC",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "UserOtps",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "ContributionSchemes",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "CFM");

            migrationBuilder.DropTable(
                name: "CFMAccounts",
                schema: "CFM");
        }
    }
}
