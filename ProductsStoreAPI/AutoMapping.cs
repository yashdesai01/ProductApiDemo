using AutoMapper;
using ProductsStoreAPI.Core.Models;
using ProductsStoreAPI.Data.Entities;
using System;

namespace ProductsStoreAPI
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            CreateMap<Guid, string>();     // the guid is parsed successfully
            CreateMap<string, Guid>();     // the guid is parsed successfully
            
            //Product
            CreateMap<Product, ProductListModel>();
            CreateMap<ProductListModel, Product>();
            CreateMap<ProductAddEditModel, Product>();
        }
    }
}