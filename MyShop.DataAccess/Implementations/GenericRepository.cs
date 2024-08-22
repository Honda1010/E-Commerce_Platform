using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? predicate=null, string? Includedword = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (Includedword != null)
            {
                // like i say table.include("Category , user")
                foreach (var item in Includedword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {  // to loop in one string or more separated by comma and exclude empty string
                    query = query.Include(item);
                }
            }
            return query.ToList();
        }

        public T GetFirstorDefault(Expression<Func<T, bool>>? predicate=null, string? Includedword = null)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (Includedword != null)
            {
                // like i say table.include("Category , user")
                foreach (var item in Includedword.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {  // to loop in one string or more separated by comma and exclude empty string
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
           _dbSet.RemoveRange(entities);
        }
    }
}
