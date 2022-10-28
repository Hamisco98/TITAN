using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Titan.Data;
using System.Linq.Dynamic.Core;
using Titan.Areas.Warehouse.Models;
using Titan.Areas.Warehouse.APIs.Repositories;
using Titan.Areas.Warehouse.APIs.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Titan.Areas.Warehouse.APIs.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsInterfaces productsInterfaces;

        public ProductsController(IProductsInterfaces productsInterfaces)
        {
            this.productsInterfaces = productsInterfaces;
        }

        [HttpGet]
        public async Task<IActionResult> GET()
        {
            try
            {
                return Ok(await productsInterfaces.GETALL());
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Get Data From Database!"));
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GETBYID(Guid id)
        {
            try
            {
                var prd = await productsInterfaces.GETBYID(id);
                if (prd == null) return NotFound();
                return Ok(prd);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Get Data From Database!"));
            }
        }

        [HttpGet("{barcode}")]
        public async Task<IActionResult> GETBYID(string barcode)
        {
            try
            {
                var prd = await productsInterfaces.GETBARCODE(barcode);
                if (prd == null) return NotFound();
                return Ok(prd);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Get Data From Database!"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<Products>> POST([FromForm] Products products)
        {
            try
            {
                if (products == null) return BadRequest("Can't Get The Header Data!");

                //if the id not null you can direct him to update the product
                var prd = await productsInterfaces.GETBYID(products.ID);
                if (prd != null) return BadRequest($"You are trying to update the product [ {prd.Name} ] with id : [ {prd.ID} ], this process can't be continued. can't POST this data to database.");


                var barcode = await productsInterfaces.GETBARCODE(products.Barcode);
                if (barcode != null)
                {
                    ModelState.AddModelError("Barcode", "This Barcode Is Already Asigned To [ " + barcode.Name + " ]!, If This barcode For That Broduct Try To Update It.");
                    return BadRequest(ModelState);
                }
                var product = await productsInterfaces.POST(products);
                return Ok(product);
            }
            catch (Exception)
            {

                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Post Data To Database!"));
            }
        }


        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Products>> PUT(Guid id, Products products)
        {
            try
            {
                if (id != products.ID) return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "The ID Mismatch!"));

                if (products == null) return BadRequest("Can't Get The Header Data!");

                if(!(products.Barcode != null && products.Barcode.Trim() != "")) return BadRequest("Barcode Is Required");
                
                var getBarcode = await productsInterfaces.GETBYID(id);
                var barcode = await productsInterfaces.GETBARCODE(products.Barcode);
                if (barcode != null && getBarcode.Barcode != products.Barcode)
                {
                    ModelState.AddModelError("Barcode", "This Barcode Is Already Asigned To [ " + barcode.Name + " ]!, If This barcode For That Broduct Try To Update It.");
                    return BadRequest(ModelState);
                }
                var product = await productsInterfaces.PUT(products);
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't PUT Data To Database!"));
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Products>> DELETE(Guid id/*, Products products*/)
        {
            try
            {
                //if(id != products.ID) return BadRequest((StatusCodes.Status500InternalServerError,"ID Mismatch!"));

                var prd = await productsInterfaces.GETBYID(id);
                if (prd == null) return NotFound($"Product With Code [ {id} ] Not Exist!.");

                await productsInterfaces.DELETE(id);
                return Ok("Deleted");
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(StatusCodes.Status500InternalServerError, "Error!. Can't Delete Data From Database!"));
            }
        }
    }
}
