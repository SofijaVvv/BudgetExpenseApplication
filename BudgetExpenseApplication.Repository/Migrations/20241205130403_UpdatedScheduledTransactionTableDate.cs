using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedScheduledTransactionTableDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduledTime",
                table: "ScheduledTransactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledDate",
                table: "ScheduledTransactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ScheduledDate",
                table: "ScheduledTransactions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ScheduledTime",
                table: "ScheduledTransactions",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
