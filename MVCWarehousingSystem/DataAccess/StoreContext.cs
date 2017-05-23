using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.DataAccess
{
    public class StoreContext : DbContext
    {
        public DbSet<StockItem> Items { get; set; }

        public StoreContext()
            : base("DefaultConnection")
        {
        }
    }
}