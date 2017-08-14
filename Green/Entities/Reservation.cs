﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Entities
{
    public class Reservation
    {
        public static int IdCounter
        {
            get { IdCounter += 1; return IdCounter; }
            set { IdCounter += value; }
        }
        public int Id { get; set; }
        public string ClientName { get; set; }
        public DateTime ReservationDate { get; set; }
        public int Seats { get; set; }
    }
}