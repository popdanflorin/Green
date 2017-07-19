using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class FoodServing
    {
        public string Id { get; set; }
        public string MealId { get; set; }
        public string FoodId { get; set; }
        public int ServingSizeInGrams { get; set; }
        public string ServingSizeDescription { get; set; }
        public int EstimatedCalories { get; set; }
    }
}