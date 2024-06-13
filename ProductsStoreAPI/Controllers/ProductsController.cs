using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsStoreAPI.Service;
using AutoMapper;
using ProductsStoreAPI.Core.Models;
using ProductsStoreAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ProductsStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;
        public ProductsController(IProductsService productsService, IMapper mapper, IHostingEnvironment environment)
        {
            _productsService = productsService;
            _mapper = mapper;
            _environment = environment;
        }


        // GET api/Products/ProductList
        [HttpGet("ProductList")]
        public async Task<IActionResult> List(int pageNo = 1, int pageSize = 10, string search = "")
        {
            var products = await _productsService.GetAll(pageNo, pageSize, search);
            products.ForEach(x => x.Images = x.Images.Replace(",", $",{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/ProductImages/").Remove(0,1));
            
            return Ok(new Response<ProductListModel> { 
                Status = "Success",
                Message = "List of products:",
                Data = products
            });
        }

        // POST api/ProductsController/Add
        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add([FromForm] ProductAddEditModel model)
        {
            var response = new Response<ProductAddEditModel> {
                Status = "Success",
                Message = "Record inserted successfully."
            };
            if (ModelState.IsValid)
            {
                if (await _productsService.CheckDuplicate(model.ProductName))
                {
                    response.Status = "Error";
                    response.Message = "Product name is duplicate, please choose another.";
                }
                else
                {
                    string uploadedFileNames = string.Empty;
                    UploadImages(model, out uploadedFileNames);
                    await _productsService.Save(model, uploadedFileNames);
                }
                return Ok(response);
            }
            return BadRequest();
        }
        
        // PUT api/Products/Update
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update([FromForm] ProductAddEditModel model)
        {
            if (ModelState.IsValid)
            {
                string uploadedFileNames = string.Empty;
                UploadImages(model, out uploadedFileNames);
                await _productsService.Save(model, uploadedFileNames);
                return Ok(new Response<ProductListModel>
                {
                    Status = "Success",
                    Message = "Record updated successfully.",
                });
            }
            return BadRequest();
        }

        // DELETE api/Products/guid
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != Guid.Empty && await _productsService.Delete(id))
            {
                return Ok(new Response<ProductListModel>
                {
                    Status = "Success",
                    Message = "Record deleted successfully.",
                });
            }
            return BadRequest();
        }

        private void UploadImages(ProductAddEditModel model, out string uploadedFileNames)
        {
            uploadedFileNames = "";
            string path = Path.Combine(_environment.ContentRootPath, "ProductImages");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (var postedFile in model.Images)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    uploadedFileNames += "," + fileName;
                }
            }
        }

    }
}
