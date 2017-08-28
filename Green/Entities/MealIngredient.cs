using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Green.Entities
{
    public class MealIngredient
    {
        public string Id { get; set; }
        public string MealId { get; set; }
        public string FoodId { get; set; }

        [ForeignKey("MealId")]
        public virtual Meal Meal { get; set; }
        [ForeignKey("FoodId")]
        public virtual Food Food{ get; set; }
    }
}