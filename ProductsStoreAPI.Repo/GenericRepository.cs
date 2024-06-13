using Microsoft.EntityFrameworkCore;
using ProductsStoreAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsStoreAPI.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _ctx;

        public GenericRepository(DbContext context)
        {
            _ctx = context;
        }

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                _ctx.Set<T>().Add(entity);
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> Update(T entity)
        {
            try
            {
                _ctx.Set<T>().Attach(entity);
                _ctx.Entry(entity).State = EntityState.Modified;
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
        public virtual async Task<bool> Delete(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            return await Task.FromResult(true);
        }

        public virtual async Task<T> Get(Guid id)
        {
            return await _ctx.Set<T>().FindAsync(id);
            //return await Task.FromResult(true);
        }
        public virtual async Task<bool> IsDuplicate(string name)
        {
            return  await _ctx.Set<Product>().AnyAsync(x=>x.ProductName == name);
        }
        public virtual async Task<bool> Delete(Guid id)
        {
            T entity = await Get(id);
            return await Delete(entity);
        }

        public virtual async Task<List<Product>> GetAll(int pageNo, int pageSize, string search)
        {
            if (pageNo == 1)
            {
                return await _ctx.Set<Product>().Where(x=>x.ProductName.Contains(search)).Take(pageSize).ToListAsync();
            }
            else
            {
                return await _ctx.Set<Product>().Where(x => x.ProductName.Contains(search)).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            }

        }
    }
}
