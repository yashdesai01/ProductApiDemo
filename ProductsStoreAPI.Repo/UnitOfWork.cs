using ProductsStoreAPI.Data;
using ProductsStoreAPI.Data.Entities;
using System.Threading.Tasks;

namespace ProductsStoreAPI.Repo
{
    public class UnitOfWork : IUnitOfWork
    {
        public ProductDbContext _context { get; private set; }

        private GenericRepository<Product> _ProductRepository;


        public UnitOfWork(ProductDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> Save()
        {
            try
            {
                int _save = await _context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }

        public GenericRepository<Product> ProductsRepository
        {
            get
            {

                if (this._ProductRepository == null)
                {
                    this._ProductRepository = new GenericRepository<Product>(_context);
                }
                return _ProductRepository;
            }
        }
    }
}
