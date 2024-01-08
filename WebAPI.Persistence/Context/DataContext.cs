using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Entities;

namespace WebAPI.Persistence.Context
{
    public class DataContext: IdentityDbContext<User, UserRole, int>
	{
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }                     // Kullanıcı Yönetimi
        public DbSet<Account> Account { get; set; }               // Hesap Yönetimi
        public DbSet<Transaction> Transaction { get; set; }       // Para Yatırma ve Çekme
        public DbSet<Transfer> Transfer { get; set; }             // Para Transferleri
        public DbSet<Loan> Loan { get; set; }                     // Kredi İşlemleri
        public DbSet<LoginModel> LoginModel { get; set; }
        public DbSet<RegisterModel> RegisterModel { get; set; }
        public DbSet<AutoPayment> AutoPayment { get; set; }       // Otomatik Ödemeler
        public DbSet<Ticket> Ticket { get; set; }                 // Destek Talepleri
		public DbSet<UserRole> UserRole { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<UserRole>().HasData(
			  new UserRole
			  {
				  Id = 1,
				  Name="admin",

			  },
			  new UserRole
			  {
				   Id = 2,
				   Name = "user",

			  });
			base.OnModelCreating(modelBuilder);
        }

		
	}
}
