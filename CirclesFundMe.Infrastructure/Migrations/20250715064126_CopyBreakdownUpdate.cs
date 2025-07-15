using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CopyBreakdownUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CopyOfCurrentAutoBreakdownAtOnboarding",
                schema: "CFM",
                table: "UserContributionSchemes",
                newName: "CopyOfCurrentBreakdownAtOnboarding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CopyOfCurrentBreakdownAtOnboarding",
                schema: "CFM",
                table: "UserContributionSchemes",
                newName: "CopyOfCurrentAutoBreakdownAtOnboarding");
        }
    }
}
