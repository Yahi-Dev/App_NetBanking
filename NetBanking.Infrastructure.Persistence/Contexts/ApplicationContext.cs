using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<SavingsAccount> SavingsAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region Tables
            modelBuilder.Entity<SavingsAccount>().ToTable("SavingsAccounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Loan>().ToTable("Loans");
            modelBuilder.Entity<CreditCard>().ToTable("CreditCards");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");
            #endregion

            #region Primary Keys
            modelBuilder.Entity<SavingsAccount>().HasKey(p => p.Id);
            modelBuilder.Entity<Transaction>().HasKey(p => p.Id);
            modelBuilder.Entity<Loan>().HasKey(p => p.Id);
            modelBuilder.Entity<CreditCard>().HasKey(p => p.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(p => p.Id);
            #endregion
        }
    }
}
