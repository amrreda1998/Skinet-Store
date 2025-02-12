using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product OldProduct, Product NewProduct);
    void DeleteProduct(Product product);
    bool CheckProductExist(int id);
    Task<bool> SaveChangesAsync();
}
