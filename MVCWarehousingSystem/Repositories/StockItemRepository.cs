using MVCWarehousingSystem.DataAccess;
using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.Repositories
{
    public class StockItemRepository
    {
        private static StoreContext db = new StoreContext();
        private static List<StockItem> itemsToBeAdded = null;
        private static List<StockItem> itemsToBeDeleted = null;

        public static List<StockItem> Items
        {
            get { return db.Items.ToList(); }
            private set { }
        }

        public static List<StockItem> StoredItems
        {
            get
            {
                List<StockItem> copy = itemsToBeAdded.ToList();
                itemsToBeAdded = null;
                return copy;
            }
            private set { }
        }

        public static List<StockItem> DeletedItems
        {
            get
            {
                List<StockItem> copy = itemsToBeDeleted.ToList();
                itemsToBeDeleted = null;
                return copy;
            }
            private set { }
        }

        public static void AddItem(StockItem item)
        {
            db.Items.Add(item);
            SaveChanges();
        }

        public static void StoreItemToBeCreated(StockItem item)
        {
            if (itemsToBeAdded == null)
                itemsToBeAdded = new List<StockItem>();

            itemsToBeAdded.Add(item);
        }

        public static void ValidateMultipleCreation()
        {
            foreach (StockItem item in itemsToBeAdded)
                db.Items.Add(item);

            if (itemsToBeAdded.Count > 0)
                SaveChanges();
        }

        public static void DeleteItem(StockItem item)
        {
            if (db.Items.Remove(item) != null)
                SaveChanges();
        }

        public static void StoreItemToBeDeleted(StockItem item)
        {
            if (itemsToBeDeleted == null)
                itemsToBeDeleted = new List<StockItem>();

            itemsToBeDeleted.Add(item);
        }

        public static void ValidateMultipleDelation()
        {
            foreach (StockItem item in itemsToBeDeleted)
                db.Items.Remove(item);

            if (itemsToBeDeleted.Count > 0)
                SaveChanges();
        }

        public static DbEntityEntry<StockItem> Item(StockItem item)
        {
            return db.Entry(item);
        }

        public static StockItem ItemByArticleNumber(int? articleNumber)
        {
            return Items.SingleOrDefault(i => i.ArticleNumber == articleNumber);
        }

        public static IEnumerable<StockItem> ItemsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<StockItem>();
            else
                return Items.Where(i => i.Name.ToLower().Contains(name.ToLower()));
        }

        public static IEnumerable<StockItem> ItemsByPrice(double? price)
        {
            return Items.Where(i => i.Price == price);
        }

        public static void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}