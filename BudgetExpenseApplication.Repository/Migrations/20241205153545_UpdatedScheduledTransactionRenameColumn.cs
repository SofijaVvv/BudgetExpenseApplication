using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScheduledTransactionRenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ScheduledTransactions",
                newName: "IsRecurring");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsRecurring",
                table: "ScheduledTransactions",
                newName: "IsActive");
        }
    }
}
