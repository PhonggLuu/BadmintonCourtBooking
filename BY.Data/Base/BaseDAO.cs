using BY.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BY.Data.Base
{
    public class BaseDAO<T> where T : class
    {
        protected readonly Net1704_221_2_BYContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseDAO()
        {
            _context = new Net1704_221_2_BYContext();
            _dbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public async Task<int> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            var tracker = _dbSet.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = _dbSet.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
        public async Task<int> RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsyc(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T? GetByCode(string code)
        {
            return _dbSet.Find(code);
        }

        public async Task<T?> GetByCodeAsync(string code)
        {
            return await _dbSet.FindAsync(code);
        }
    }
}
