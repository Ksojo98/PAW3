namespace PAW3.Models.Entities.Productdb
{
    public partial class Product
    {
        public string RatingClass { get; set; } = string.Empty;
        public string TimeClass { get; set; } = string.Empty;

        public int Time
        {
            get
            {
                if (!LastModified.HasValue)
                    return 0;

                return (DateTime.Now - LastModified.Value).Days;
            }
        }
    }
}