using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace ProductsStoreAPI.Models
{
    public class Response<T> where T: class
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
