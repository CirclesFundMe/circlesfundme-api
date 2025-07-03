using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemeTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanManagementFee",
                schema: "CFM",
                table: "ContributionSchemes",
                newName: "ProcessingFeePercent");

            migrationBuilder.AlterColumn<string>(
                name: "SchemeType",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<double>(
                name: "DownPaymentPercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "EligibleLoanPercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtraEnginePercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtraTyrePercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "InsurancePerAnnumPercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LoanManagementFeePercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Type",
                schema: "CFM",
                table: "Notifications",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notifications_Type",
                schema: "CFM",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DownPaymentPercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.DropColumn(
                name: "EligibleLoanPercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.DropColumn(
                name: "ExtraEnginePercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.DropColumn(
                name: "ExtraTyrePercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.DropColumn(
                name: "InsurancePerAnnumPercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.DropColumn(
                name: "LoanManagementFeePercent",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.RenameColumn(
                name: "ProcessingFeePercent",
                schema: "CFM",
                table: "ContributionSchemes",
                newName: "LoanManagementFee");

            migrationBuilder.AlterColumn<int>(
                name: "SchemeType",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
