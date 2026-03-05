using PAW3.Models.Entities.Productdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3.Core.Domain
{
    public class ProductDomain
    {
        private readonly Product _product;


        public ProductDomain(Product product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }

        public ProductDomain ApplyRules()
            => CleanRating()
               .ApplyRatingClass()
               .ApplyTimeClass();

        public ProductDomain CleanRating()
        {
            _product.Rating = !_product.Rating.HasValue
                ? 0
                : _product.Rating < 0 ? 0
                : _product.Rating > 5 ? 5
                : _product.Rating;

            return this;
        }

        public ProductDomain ApplyRatingClass()
        {
            var rating = _product.Rating ?? 0;

            _product.RatingClass =
                rating >= 4 ? "A" :
                rating >= 3 ? "B" :
                rating >= 2 ? "C" :
                rating >= 1 ? "D" :
                "unrated";

            return this;
        }

        public ProductDomain ApplyTimeClass()
        {
            var daysOld = !_product.LastModified.HasValue
                ? double.MaxValue
                : (DateTime.UtcNow - _product.LastModified.Value).TotalDays;

            _product.TimeClass =
                daysOld <= 7 ? "new" :
                daysOld <= 30 ? "recent" :
                "old";

            return this;
        }
    }
}