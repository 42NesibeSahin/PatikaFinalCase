using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Domain.Common;

namespace WebAPI.Application.Interfaces.Repositories
{
	public interface IGenericRepository<TEntity> where TEntity : class, IEntity
	{
		Task<TEntity> GetByIDAsync(int id);
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> AddAsync(TEntity entity);

		Task<TEntity> UpdateAsync(TEntity entity);

		Task DeleteAsync(int id);

		IQueryable<TEntity> GetWhereQuery(Expression<Func<TEntity, bool>>? predicate);

		Task<bool> Exist(int id);
		IQueryable<TEntity> AsQueryable();

		Task<int> Save();
		Task Rollback();

		IExecutionStrategy CreateExecutionStrategy();
		Task<IDbContextTransaction> BeginTransactionAsync();

		Task<TEntity?> GetByIdLockedAsync(int id);

		Task TransactionCommitAsync(IDbContextTransaction transaction);



	}
}
