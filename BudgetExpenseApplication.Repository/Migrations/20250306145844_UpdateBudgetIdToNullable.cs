using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBudgetIdToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                table: "Transactions",
                type: "int",
                nullable: true,  // This makes it nullable
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                table: "Transactions",
                type: "int",
                nullable: false,  // This reverts it to non-nullable
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }

}
