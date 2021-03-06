﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Image
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public bool isCover { get; set; }
        public string RestaurantId { get; set; }
        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }
    }
}