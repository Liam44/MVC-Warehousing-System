using MVCWarehousingSystem.DataAccess;
using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.Repositories
{
    public class StockItemRepository : IDisposable
    {
        StoreContext db = new StoreContext();

        public List<StockItem> Items
        {
            get { return db.Items.ToList(); }
            private set { }
        }

        public void AddItem(StockItem item)
        {
            db.Items.Add(item);
            SaveChanges();
        }

        public void DeleteItem(StockItem item)
        {
            if (db.Items.Remove(item) != null)
                SaveChanges();
        }

        public DbEntityEntry<StockItem> Item(StockItem item)
        {
            return db.Entry(item);
        }

        public StockItem ItemByArticleNumber(int? articleNumber)
        {
            return Items.SingleOrDefault(i => i.ArticleNumber == articleNumber);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}