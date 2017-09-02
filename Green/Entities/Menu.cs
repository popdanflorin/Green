using Green.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Green.Entities
{
    public class Menu
    {
        public string Id { get; set; }
        public string RestaurantId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public string StartDateDisplay
        {
            get
            {
                return StartDate.ToString("dd-MMM-yyyy");
            }
        }
        public string EndDateDisplay
        {
            get
            {
                return EndDate.ToString("dd-MMM-yyyy");
            }
        }

        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }

    }
}