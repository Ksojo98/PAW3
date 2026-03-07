using PAW3.Data.Repositories;
using PAW3.Models.DTO;
using PAW3.Models.Entities.Productdb;
using PAW3.Core.Domain;

namespace PAW3.Core.BusinessLogic;

public interface IProductBusiness
{
    /// <summary>
    /// Deletes the product associated with the product id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteProductAsync(int id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ProductDTO> GetProducts(int? id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    Task<bool> SaveProductAsync(Product product);
}

public class ProductBusiness(IRepositoryProduct repositoryProduct) : IProductBusiness
{
    /// </inheritdoc>
    public async Task<bool> SaveProductAsync(Product product)
    {
        // que tengan mas de 5 quantity
        // sabado o domingo solo puedo salvar de 8 a 12
        return await repositoryProduct.UpdateAsync(product);
    }

    /// </inheritdoc>
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await repositoryProduct.FindAsync(id);
        return await repositoryProduct.DeleteAsync(product);
    }
    
    /// </inheritdoc>
    public async Task<ProductDTO> GetProducts(int? id)
    {
        var hasId = id.HasValue;
        var productDto = new ProductDTO();

        var products = !hasId
            ? await repositoryProduct.ReadAsync()
            : [await repositoryProduct.FindAsync((int)id)];

        if (products == null)
        {
            productDto.Products = [];
            return productDto;
        }

        var productList = products.Where(p => p != null).ToList();

        if (!productList.Any())
        {
            productDto.Products = productList;
            return productDto;
        }

        var domain = new ProductDomain();
        productList = domain.ApplyBusinessRules(productList).ToList();

        productList = new ProductFluent(productList)
            .ApplyRatingClass()
            .ApplyTimeClass()
            .Build()
            .ToList();

        if (!hasId)
        {
            productDto.Summaries.AddRange(
                productList
                    .GroupBy(x => new { x.RatingClass, x.TimeClass })
                    .Select(g => new ProductSummary
                    {
                        RatingClass = g.Key.RatingClass,
                        TimeClass = g.Key.TimeClass,
                        Count = g.Count()
                    })
                    .OrderBy(x => x.RatingClass)
                    .ThenBy(x => x.TimeClass)
            );
        }

        productDto.Products = productList;
        return productDto;
    }
}

