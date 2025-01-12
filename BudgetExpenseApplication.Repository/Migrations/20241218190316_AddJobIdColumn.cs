using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddJobIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "ScheduledTransactions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "ScheduledTransactions");
        }
    }
}
