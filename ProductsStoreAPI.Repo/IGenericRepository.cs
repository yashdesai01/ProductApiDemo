using ProductsStoreAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ProductsStoreAPI.Repo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<T> Get(Guid id);
        Task<bool> Delete(Guid id);
        Task<List<Product>> GetAll(int pageNo, int pageSize, string search);
    }
}