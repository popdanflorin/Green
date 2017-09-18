using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Green.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Green.Models
{
    public class RestaurantPercentages
    {
        public Restaurant Restaurant { get; set; }
        public List<int> Percentages { get; set; }
    }
}