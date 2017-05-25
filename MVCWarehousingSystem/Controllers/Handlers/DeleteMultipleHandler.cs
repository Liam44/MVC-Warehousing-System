using MVCWarehousingSystem.Models;
using System.Collections.Generic;

namespace MVCWarehousingSystem.Controllers.Handlers
{
    internal class DeleteMultipleHandler
    {
        public static Dictionary<StockItem, bool> ItemsToBeDeleted { get; set; }

        public static void Initiate(IEnumerable<StockItem> items)
        {
            if (ItemsToBeDeleted == null)
                ItemsToBeDeleted = new Dictionary<StockItem, bool>();
            else
                ItemsToBeDeleted.Clear();

            foreach (StockItem item in items)
                ItemsToBeDeleted.Add(item, false);
        }
    }
}