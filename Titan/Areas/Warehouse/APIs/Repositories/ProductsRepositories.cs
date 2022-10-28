using Microsoft.EntityFrameworkCore;
using Titan.Areas.Warehouse.APIs.Interfaces;
using Titan.Areas.Warehouse.Models;
using Titan.Data;

namespace Titan.Areas.Warehouse.APIs.Repositories
{
    public class ProductsRepositories : IProductsInterfaces
    {
        private readonly ApplicationDbContext context;

        public ProductsRepositories(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Products>> GETALL()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Products> GETBYID(Guid id)
        {
            return await context.Products.FirstOrDefaultAsync(e => e.ID == id);
        }

        public async Task<Products> POST(Products products)
        {
            var product = await context.Products.AddAsync(products);
            await context.SaveChangesAsync();
            return product.Entity;
        }

        public async Task<Products> PUT(Products products)
        {
            var prd = await context.Products.FirstOrDefaultAsync(p => p.ID == products.ID);
            if (prd != null)
            {
                prd.ID = products.ID;
                prd.Name = products.Name;
                prd.Price = products.Price;
                prd.Cost = products.Cost;
                prd.Barcode = products.Barcode;
                prd.Date = prd.Date;

                await context.SaveChangesAsync();
                return prd;
            }
            return null;
        }
        
        public async Task<Products> DELETE(Guid id)
        {
            var prd = await context.Products.FirstOrDefaultAsync(p => p.ID == id);
            if (prd != null)
            {
                context.Products.Remove(prd);
                await context.SaveChangesAsync();
                return prd;
            }
            return null;
        }

        public async Task<IEnumerable<Products>> SEARCH(string name, string barcode, double price, double netPrice)
        {
            IQueryable<Products> query = context.Products;
            if (!string.IsNullOrEmpty(name)) query = query.Where(p => p.Name == name);
            if (!string.IsNullOrEmpty(barcode)) query = query.Where(p => p.Barcode == barcode);
            if (price > 0) query = query.Where(p => p.Price == price);
            if (netPrice > 0) query = query.Where(p => p.Cost == netPrice);
            return await query.ToListAsync();
        }

        public async Task<Products> GETBARCODE(string barcode)
        {
            if(barcode == null) return null;
            return await context.Products.FirstOrDefaultAsync(e => e.Barcode == barcode);
        }
    }
}
