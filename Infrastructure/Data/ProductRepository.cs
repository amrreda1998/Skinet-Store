using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public bool CheckProductExist(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
    {
        var AllProducts = await context.Products.ToListAsync();
        return AllProducts;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product OldProduct, Product NewProduct)
    {
        OldProduct.Name = NewProduct.Name.IsNullOrEmpty() ? OldProduct.Name : NewProduct.Name;
        OldProduct.Description = NewProduct.Description.IsNullOrEmpty() ? OldProduct.Description : NewProduct.Description;
        OldProduct.PictureUrl = NewProduct.PictureUrl.IsNullOrEmpty() ? OldProduct.PictureUrl : NewProduct.PictureUrl;
        OldProduct.Type = NewProduct.Type.IsNullOrEmpty() ? OldProduct.Type : NewProduct.Type;
        OldProduct.Brand = NewProduct.Brand.IsNullOrEmpty() ? OldProduct.Brand : NewProduct.Brand;
        OldProduct.Price = NewProduct.Price < 0 ? OldProduct.Price : NewProduct.Price;
        OldProduct.QuantityInStock = NewProduct.QuantityInStock < 0 ? OldProduct.QuantityInStock : NewProduct.QuantityInStock;
    }

}
