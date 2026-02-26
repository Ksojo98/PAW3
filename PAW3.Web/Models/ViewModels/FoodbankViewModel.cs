namespace PAW3.Web.Models.ViewModels
{
    public class FoodBankViewModel
    {
        public int FoodItemId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public string Unit { get; set; } = string.Empty;

        public int QuantityInStock { get; set; }
        public DateOnly? ExpirationDate { get; set; }

        public bool IsPerishable { get; set; }
        public int CaloriesPerServing { get; set; }

        public string Ingredients { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}