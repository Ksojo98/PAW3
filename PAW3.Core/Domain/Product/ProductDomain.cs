using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class ProductDomain
    {
        private readonly ProductPipeline _pipeline;

        public ProductDomain()
        {
            _pipeline = new ProductPipeline(new List<IProductRuleStep>
            {
                new DefaultRatingStep(),
                new RatingClassStep(),
                new TimeClassStep()
            });
        }

        public IEnumerable<Product> ApplyBusinessRules(IEnumerable<Product> products)
        {
            return _pipeline.Execute(products);
        }
    }
}