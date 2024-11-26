using BudgetExpenseSystem.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetExpenseSystem.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {}
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BudgetType> BudgetTypes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
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
            entity.HasIndex(u => u.UserId).IsUnique();
            
            entity.HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Account>(a => a.UserId)
                .IsRequired();
        });

        
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasOne(b => b.User)
                .WithMany() 
                .HasForeignKey(b => b.UserId)
                .IsRequired();
            
            entity.HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .IsRequired();
            
            entity.HasOne(b => b.BudgetType)
                .WithMany() 
                .HasForeignKey(b => b.BudgetTypeId)
                .IsRequired();
        });

        
        modelBuilder.Entity<Transaction>(entity =>
        {
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


        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .IsRequired();

        });
        
        base.OnModelCreating(modelBuilder);
    }
}