using ClinicAppointmentManager.Core.Constants;
using ClinicAppointmentManager.Core.Interfaces;
using ClinicAppointmentManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ClinicAppointmentManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ClinicDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ClinicDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id, string includes = "")
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var include in includes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }
            var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, typeof(T).Name + "Id") == id);

            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string icludes = "", int page = 1)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(icludes))
            {
                foreach (var include in icludes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include.Trim());
                }
            }

            if (page > 0)
            {
                int pageSize = ConstantNumbers.DefaultPageSize;
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int Id)
        {
            var entity = await _dbSet.FindAsync(Id);
            if (entity == null)
            {
                throw new ArgumentException($"Entity with id {Id} not found.");
            }
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null;
        }
        
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.CountAsync(predicate);
        }
    }
}
