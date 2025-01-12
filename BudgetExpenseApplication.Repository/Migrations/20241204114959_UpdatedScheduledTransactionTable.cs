using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScheduledTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "ScheduledTime",
                table: "ScheduledTransactions",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                table: "ScheduledTransactions");
        }
    }
}
