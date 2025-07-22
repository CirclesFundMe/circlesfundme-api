using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CirclesFundMe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTblUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                schema: "CFM",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                schema: "CFM",
                table: "Payments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                schema: "CFM",
                table: "Payments",
                column: "Reference");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Reference",
                schema: "CFM",
                table: "Payments",
                column: "Reference",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                schema: "CFM",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_Reference",
                schema: "CFM",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "Reference",
                schema: "CFM",
                table: "Payments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                schema: "CFM",
                table: "Payments",
                column: "Id");
        }
    }
}
