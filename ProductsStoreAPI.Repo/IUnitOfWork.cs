using ProductsStoreAPI.Data;
using ProductsStoreAPI.Data.Entities;
using System.Threading.Tasks;

namespace ProductsStoreAPI.Repo
{
    public interface IUnitOfWork
    {
        public ProductDbContext _context { get; }
        Task<bool> Save();
        GenericRepository<Product> ProductsRepository { get; }
    }
}
