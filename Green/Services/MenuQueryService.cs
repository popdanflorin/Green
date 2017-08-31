using Green.Entities;
using Green.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Green.Services
{
    public class MenuQueryService
    {
        private ApplicationDbContext ctx = new ApplicationDbContext();

        public List<Menu> GetMenus()
        {
            return ctx.Menus.ToList();
        }
    }
}