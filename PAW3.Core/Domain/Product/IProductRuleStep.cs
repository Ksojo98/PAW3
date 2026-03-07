using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public interface IProductRuleStep
    {
        Product Execute(Product product);
    }
}