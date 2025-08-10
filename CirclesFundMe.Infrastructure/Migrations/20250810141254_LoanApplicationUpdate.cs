using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanApplicationUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Breakdown",
                schema: "CFM",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentEligibleAmount",
                schema: "CFM",
                table: "LoanApplications",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Scheme",
                schema: "CFM",
                table: "LoanApplications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Breakdown",
                schema: "CFM",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "CurrentEligibleAmount",
                schema: "CFM",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Scheme",
                schema: "CFM",
                table: "LoanApplications");
        }
    }
}
