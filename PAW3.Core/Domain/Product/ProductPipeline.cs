using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class ProductPipeline
    {
        private readonly IEnumerable<IProductRuleStep> _steps;

        public ProductPipeline(IEnumerable<IProductRuleStep> steps)
        {
            _steps = steps;
        }

        public IEnumerable<Product> Execute(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                var current = product;

                foreach (var step in _steps)
                {
                    current = step.Execute(current);
                }

                yield return current;
            }
        }
    }
}