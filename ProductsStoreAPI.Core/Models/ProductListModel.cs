using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace ProductsStoreAPI.Core.Models
{
    public class ProductListModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Images { get; set; }
    }
    public class ProductAddEditModel
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
