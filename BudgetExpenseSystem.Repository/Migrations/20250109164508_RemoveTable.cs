using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetExpenseSystem.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_BudgetTypes_BudgetTypeId",
                table: "Budgets");

            migrationBuilder.DropTable(
                name: "BudgetTypes");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_BudgetTypeId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "BudgetTypeId",
                table: "Budgets");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ScheduledTransactions",
                type: "varchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ScheduledTransactions");

            migrationBuilder.AddColumn<int>(
                name: "BudgetTypeId",
                table: "Budgets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BudgetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTypeId",
                table: "Budgets",
                column: "BudgetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_BudgetTypes_BudgetTypeId",
                table: "Budgets",
                column: "BudgetTypeId",
                principalTable: "BudgetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
