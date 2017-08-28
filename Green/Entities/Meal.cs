﻿using Green.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Meal
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }    
        public MealType Type { get; set; }

        public string TypeDisplay
        {
            get
            {
                return Type.ToString();
            }
        }
    }
}