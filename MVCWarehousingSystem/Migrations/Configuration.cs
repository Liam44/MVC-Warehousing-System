namespace MVCWarehousingSystem.Migrations
{
    using MVCWarehousingSystem.Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCWarehousingSystem.DataAccess.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCWarehousingSystem.DataAccess.StoreContext context)
        {
            context.Items.AddOrUpdate(i => i.ArticleNumber,
                 new StockItem { Name = "Motherboard 1", Price = 1234.50, ShelfPosition = "H01", Quantity = 0, Description = "Hardware" },
                 new StockItem { Name = "Motherboard 2", Price = 2345.60, ShelfPosition = "H02", Quantity = 3, Description = "Hardware" },
                 new StockItem { Name = "Motherboard 3", Price = 3456.70, ShelfPosition = "H03", Quantity = 4, Description = "Hardware" },
                 new StockItem { Name = "CPU 1", Price = 1234.50, ShelfPosition = "H04", Quantity = 2, Description = "Hardware" },
                 new StockItem { Name = "CPU 2", Price = 2345.60, ShelfPosition = "H05", Quantity = 5, Description = "Hardware" },
                 new StockItem { Name = "CPU 3", Price = 2345.60, ShelfPosition = "H06", Quantity = 8, Description = "Hardware" },
                 new StockItem { Name = "Keyboard 1", Price = 123.40, ShelfPosition = "P01", Quantity = 10, Description = "Peripheral" },
                 new StockItem { Name = "keyboard 2", Price = 234.50, ShelfPosition = "P02", Quantity = 70, Description = "Peripheral" },
                 new StockItem { Name = "Mouse 1", Price = 12.30, ShelfPosition = "P03", Quantity = 64, Description = "Peripheral" },
                 new StockItem { Name = "Mouse 2", Price = 23.40, ShelfPosition = "P04", Quantity = 56, Description = "Peripheral" },
                 new StockItem { Name = "Speakers 1", Price = 1234.50, ShelfPosition = "P05", Quantity = 6, Description = "Peripheral" },
                 new StockItem { Name = "Speakers 2", Price = 234.50, ShelfPosition = "P06", Quantity = 6, Description = "Peripheral" },
                 new StockItem { Name = "Suite Office", Price = 12345.60, ShelfPosition = "S01", Quantity = 200, Description = "Software" },
                 new StockItem { Name = "Photoshop", Price = 23456.70, ShelfPosition = "S02", Quantity = 70, Description = "Software" },
                 new StockItem { Name = "Screws", Price = 123.40, ShelfPosition = "O01", Quantity = 90, Description = "Others" });
        }
    }
}
