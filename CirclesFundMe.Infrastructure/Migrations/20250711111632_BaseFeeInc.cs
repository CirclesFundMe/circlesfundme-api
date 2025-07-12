using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BaseFeeInc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DownPaymentPercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "BaseFee",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseFee",
                schema: "CFM",
                table: "ContributionSchemes");

            migrationBuilder.AlterColumn<double>(
                name: "DownPaymentPercent",
                schema: "CFM",
                table: "ContributionSchemes",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
