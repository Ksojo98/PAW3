using PAW3.Models.Entities.Productdb;

namespace PAW3.Core.Domain { 
    public class TimeClassStep : IProductRuleStep
    {
        public Product Execute(Product product)
        {
            if (product.Time <= 15)
                product.TimeClass = "A";
            else if (product.Time <= 25)
                product.TimeClass = "B";
            else
                product.TimeClass = "C";

            return product;
        }
    }
}
