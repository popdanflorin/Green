using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Green.Entities
{
    public class MenuMeal
    {
        public MenuMeal() { }
        public MenuMeal(string _Id, string _MenuId, string _MealId)
        {
            Id = _Id;
            MenuId = _MenuId;
            MealId = _MealId;
        }
        public string Id { get; set; }
        public string MenuId { get; set; }
        public string MealId { get; set; }

        [ForeignKey("MenuId")]
        public virtual Menu Menu{ get; set; }
        [ForeignKey("MealId")]
        public virtual Meal Meal { get; set; }
    }
}