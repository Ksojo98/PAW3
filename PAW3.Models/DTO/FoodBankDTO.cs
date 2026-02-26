using PAW3.Data.Foodbankdb.Models;

namespace PAW3.Models.DTO
{
    public class FoodBankDTO
    {
        public IEnumerable<FoodItem> Products { get; set; } = [];

    }
}
