using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Domain.Entities;
using WebAPI.Persistence.Context;

namespace WebAPI.Persistence.Repositories
{
	public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
	{
		public TransactionRepository(DataContext context) : base(context)
		{

		}

		public async Task UpdateAsync(Transaction transaction)
		{
			// Update account logic...
			_context.Transaction.Update(transaction);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				// Handle concurrency issue here
				// Reload the entity, reapply changes, and save again
			}
		}
	}
}
