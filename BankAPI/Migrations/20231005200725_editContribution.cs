using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankAPI.Migrations
{
    /// <inheritdoc />
    public partial class editContribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Money",
                table: "Contributions",
                newName: "money");

            migrationBuilder.RenameColumn(
                name: "Money",
                table: "Accounts",
                newName: "money");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Contributions",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "Money",
                table: "Contributions",
                sql: "Money >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "Money",
                table: "Accounts",
                sql: "Money >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Money",
                table: "Contributions");

            migrationBuilder.DropCheckConstraint(
                name: "Money",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "money",
                table: "Contributions",
                newName: "Money");

            migrationBuilder.RenameColumn(
                name: "money",
                table: "Accounts",
                newName: "Money");

            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "Contributions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
