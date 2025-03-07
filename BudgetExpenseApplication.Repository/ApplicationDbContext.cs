using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseApplication.Repository;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Role> Roles { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Account> Accounts { get; set; }
	public DbSet<Budget> Budgets { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<ScheduledTransaction> ScheduledTransactions { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<User>(entity =>
		{
			entity.HasIndex(u => u.Email).IsUnique();

			entity.HasOne(u => u.Role)
				.WithMany()
				.HasForeignKey(u => u.RoleId)
				.IsRequired();
		});

		modelBuilder.Entity<Account>(entity =>
		{
			entity.Property(a => a.Balance)
				.HasColumnType("decimal(18, 2)");
		});


		modelBuilder.Entity<Budget>(entity =>
		{
			entity.Property(b => b.Amount)
				.HasColumnType("decimal(18, 2)");

			entity.HasOne(b => b.Category)
				.WithMany()
				.HasForeignKey(b => b.CategoryId)
				.IsRequired();

			entity.HasOne(b => b.User)
				.WithMany()
				.HasForeignKey(b => b.UserId)
				.IsRequired();

		});


		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.Property(t => t.Amount)
				.HasColumnType("decimal(18, 2)");

			entity.HasOne(t => t.Account)
				.WithMany()
				.HasForeignKey(t => t.AccountId)
				.IsRequired();

			entity.HasOne(t => t.Budget)
				.WithMany()
				.HasForeignKey(t => t.BudgetId)
				.IsRequired(false);
		});

		modelBuilder.Entity<ScheduledTransaction>(entity =>
		{
			entity.Property(t => t.Amount)
				.HasColumnType("decimal(18, 2)");

			entity.HasOne(t => t.Account)
				.WithMany()
				.HasForeignKey(t => t.AccountId)
				.IsRequired();

			entity.HasOne(t => t.Category)
				.WithMany()
				.HasForeignKey(t => t.CategoryId)
				.IsRequired();

			entity.HasOne(t => t.Budget)
				.WithMany()
				.HasForeignKey(t => t.BudgetId)
				.IsRequired();
		});

		base.OnModelCreating(modelBuilder);
	}
}
