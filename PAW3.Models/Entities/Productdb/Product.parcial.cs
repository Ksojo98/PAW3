using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW3.Models.Entities.Productdb
{
    public partial class Product
    {
        public string RatingClass { get; set; } = "unrated";
        public string TimeClass { get; set; } = "old";
    }
}
