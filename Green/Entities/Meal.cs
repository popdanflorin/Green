using Green.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Meal
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string Details { get; set; }
        public string PreparationDetails { get; set; }

        public DateTime PlannedTime { get; set; }
        public DateTime ActualTime { get; set; }

        public MealType Type { get; set; }
        public MealStatus Status { get; set; }
        public MealRating Rating { get; set; }
        
        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
        public string RatingDisplay
        {
            get
            {
                return Rating.ToString();
            }
        }
        public string StatusDisplay
        {
            get
            {
                return Status.ToString();
            }
        }
    }
}