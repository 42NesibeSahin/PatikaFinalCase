using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.Interfaces.Repositories;
using WebAPI.Domain.Common;
using WebAPI.Persistence.Context;

namespace WebAPI.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class,IEntity
    {
        protected DataContext _context = new DataContext();
        protected DbSet<T> _entity => _context.Set<T>();

        public GenericRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> AddAsync(T entity)
        {
            await _entity.AddAsync(entity);
            //Save();
			return entity;
        }
	

		public async Task DeleteAsync(int id)
        {
            var entity = await GetByIDAsync(id);
            if (entity != null)
            {
                _entity.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<T> GetByIDAsync(int id)
        {
            return await _entity.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> GetWhereQuery(Expression<Func<T, bool>>? predicate)
        {
            return _entity.Where(predicate);
        }
        public async Task<bool> Exist(int id)
        {
            return await _entity.AnyAsync(c => c.Id == id);
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _entity.Update(entity);
            //Save();
            return entity;
        }
        
        public IQueryable<T> AsQueryable()
        {
            return (_entity.AsQueryable());
        }


        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();        
        }

        public async Task Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(async x => await x.ReloadAsync());
        }

		//public async Task<int> SaveAsync()
		//{
		//	return await _context.SaveChangesAsync();
		//}

	}
}
