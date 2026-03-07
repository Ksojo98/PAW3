
using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class ProductFluent
    {
        private readonly IEnumerable<Product> _products;

        public ProductFluent(IEnumerable<Product> products)
        {
            _products = products;
        }

        public ProductFluent ApplyRatingClass()
        {
            foreach (var product in _products)
            {
                var rating = product.Rating ?? 3;

                if (rating < 2)
                    product.RatingClass = "D";
                else if (rating < 3.5m)
                    product.RatingClass = "C";
                else if (rating < 4.5m)
                    product.RatingClass = "B";
                else
                    product.RatingClass = "A";
            }

            return this;
        }

        public ProductFluent ApplyTimeClass()
        {
            foreach (var product in _products)
            {
                if (product.Time <= 15)
                    product.TimeClass = "A";
                else if (product.Time <= 25)
                    product.TimeClass = "B";
                else
                    product.TimeClass = "C";
            }

            return this;
        }

        public IEnumerable<Product> Build()
        {
            return _products;
        }
    }
}
