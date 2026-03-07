using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain
{
    public class ProductDomain
    {
        public IEnumerable<Product> ApplyBusinessRules(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                product.Rating = product.Rating ?? 3;
                product.RatingClass = GetRatingClass(product.Rating.Value);
                product.TimeClass = GetTimeClass(product.Time);

                yield return product;
            }
        }

        private string GetRatingClass(decimal rating)
        {
            if (rating < 2) return "D";
            if (rating >= 2 && rating < 3.5m) return "C";
            if (rating >= 3.6m && rating < 4.5m) return "B";
            return "A";
        }

        private string GetTimeClass(int time)
        {
            if (time <= 15) return "A";
            if (time <= 25) return "B";
            return "C";
        }
    }
}