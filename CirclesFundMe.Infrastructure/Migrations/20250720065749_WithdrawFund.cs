using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WithdrawFund : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionReference",
                schema: "CFM",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                schema: "CFM",
                table: "Transactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionReference",
                schema: "CFM",
                table: "Transactions",
                column: "TransactionReference");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionReference",
                schema: "CFM",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SessionId",
                schema: "CFM",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionReference",
                schema: "CFM",
                table: "Transactions",
                column: "TransactionReference",
                unique: true,
                filter: "[TransactionReference] IS NOT NULL");
        }
    }
}
