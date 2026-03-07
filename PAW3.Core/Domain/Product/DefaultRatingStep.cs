using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class DefaultRatingStep : IProductRuleStep
    {
        public Product Execute(Product product)
        {
            product.Rating = product.Rating ?? 3;
            return product;
        }
    }
}