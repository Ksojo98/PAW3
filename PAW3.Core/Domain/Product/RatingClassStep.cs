using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class RatingClassStep : IProductRuleStep
    {
        public Product Execute(Product product)
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

            return product;
        }
    }
}