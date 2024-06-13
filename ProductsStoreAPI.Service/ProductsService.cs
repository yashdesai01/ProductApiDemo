using AutoMapper;
using ProductsStoreAPI.Core.Models;
using ProductsStoreAPI.Data.Entities;
using ProductsStoreAPI.Repo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductsStoreAPI.Service
{
    public interface IProductsService
    {
        Task<List<ProductListModel>> GetAll(int pageNo, int pageSize, string search);
        Task<bool> Delete(Guid id);
        Task<ProductListModel> Get(Guid id);
        Task<bool> Save(ProductAddEditModel model, string filenames = "");
        Task<bool> CheckDuplicate(string name);
    }

    public class ProductsService : IProductsService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductsService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> CheckDuplicate(string name)
        {
            return await _uow.ProductsRepository.IsDuplicate(name);
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _uow.ProductsRepository.Get(id);
            if (entity != null)
            {
                await _uow.ProductsRepository.Delete(entity);
                await _uow.Save();
                return true;
            }
            return false;
        }

        public async Task<ProductListModel> Get(Guid id)
        {
           return _mapper.Map<ProductListModel>(await _uow.ProductsRepository.Get(id));
        }

        public async Task<List<ProductListModel>> GetAll(int pageNo, int pageSize, string search)
        {
            return _mapper.Map<List<ProductListModel>>(await _uow.ProductsRepository.GetAll(pageNo, pageSize, search));
        }
        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
        public async Task<bool> Save(ProductAddEditModel model, string filenames = "")
        {
            if (model.Id == Guid.Empty)
            {
                var entity = _mapper.Map<Product>(model);
                entity.Images = filenames;
                entity.CreatedOn = DateTime.UtcNow;
                await _uow.ProductsRepository.Add(entity);

                if (await _uow.Save())
                {
                    return true;
                }
            }
            else
            {
                var entity = _mapper.Map<Product>(model);
                entity.Images = filenames;
                entity.UpdatedOn = DateTime.UtcNow;
                await _uow.ProductsRepository.Update(entity);
                if (await _uow.Save())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
