using MVCWarehousingSystem.Models;
using System.Data.Entity;

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