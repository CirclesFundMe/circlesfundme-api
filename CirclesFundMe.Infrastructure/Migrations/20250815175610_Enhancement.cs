using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Enhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "CFM",
                table: "ApprovedLoans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageTemplates_Type",
                schema: "CFM",
                table: "MessageTemplates",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedLoans_UserId",
                schema: "CFM",
                table: "ApprovedLoans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedLoans_Users_UserId",
                schema: "CFM",
                table: "ApprovedLoans",
                column: "UserId",
                principalSchema: "CFM",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovedLoans_Users_UserId",
                schema: "CFM",
                table: "ApprovedLoans");

            migrationBuilder.DropIndex(
                name: "IX_MessageTemplates_Type",
                schema: "CFM",
                table: "MessageTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ApprovedLoans_UserId",
                schema: "CFM",
                table: "ApprovedLoans");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "CFM",
                table: "ApprovedLoans");
        }
    }
}
