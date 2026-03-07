using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class ProductFluent
    {
        private IEnumerable<Product> _products;

        public ProductFluent(IEnumerable<Product> products)
        {
            _products = products;
        }

        public ProductFluent ApplyBusinessRules()
        {
            var domain = new ProductDomain();
            _products = domain.ApplyBusinessRules(_products).ToList();
            return this;
        }

        public IEnumerable<Product> Build()
        {
            return _products;
        }
    }
}