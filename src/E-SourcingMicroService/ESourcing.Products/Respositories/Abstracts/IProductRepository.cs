using ESourcing.Products.Entities;

namespace ESourcing.Products.Respositories.Abstracts;

public interface IProductRepository {
	Task<IEnumerable<Product>> GetProducts();
	Task<Product> GetProduct(String id);
	Task<IEnumerable<Product>> GetProductByName(String name);
	Task<IEnumerable<Product>> GetProductByCategory(String categoryName);


	Task Create(Product product);
	Task<Boolean> Update(Product product);
	Task<Boolean> Delete(String id);
}