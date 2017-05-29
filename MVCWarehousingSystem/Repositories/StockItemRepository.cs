using MVCWarehousingSystem.DataAccess;
using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace MVCWarehousingSystem.Repositories
{
    public class StockItemRepository : IDisposable
    {
        private StoreContext db = new StoreContext();

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

        public void AddItems(List<StockItem> items)
        {
            foreach (StockItem item in items)
                db.Items.Add(item);

            if (items.Count > 0)
                SaveChanges();
        }

        public void DeleteItem(StockItem item)
        {
            if (db.Items.Remove(item) != null)
                SaveChanges();
        }

        public void DeleteItems(List<StockItem> items)
        {
            foreach (StockItem item in items)
                db.Items.Remove(ItemByArticleNumber(item.ArticleNumber));

            if (items.Count > 0)
                SaveChanges();
        }

        public void ChangeItemState(StockItem item, EntityState state)
        {
            db.Entry(item).State = state;
            SaveChanges();
        }

        public StockItem ItemByArticleNumber(int? articleNumber)
        {
            return Items.SingleOrDefault(i => i.ArticleNumber == articleNumber);
        }

        public IEnumerable<StockItem> ItemsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<StockItem>();
            else
                return Items.Where(i => i.Name.ToLower().Contains(name.ToLower()));
        }

        public IEnumerable<StockItem> ItemsByPrice(double? price)
        {
            return Items.Where(i => i.Price == price);
        }

        public List<StockItem> Deserialize(XElement elements)
        {
            List<StockItem> importedArticles = new List<StockItem>();

            foreach (XElement element in elements.Elements())
            {
                try
                {
                    StockItem stockItem = new StockItem
                    {
                        Name = (string)element.Attribute("articlename"),
                        Price = (double)element.Attribute("price"),
                        ShelfPosition = (string)element.Attribute("shelfposition"),
                        Quantity = (int)element.Attribute("quantity"),
                        Description = (string)element.Attribute("description")
                    };

                    importedArticles.Add(stockItem);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            AddItems(importedArticles);

            return importedArticles;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}