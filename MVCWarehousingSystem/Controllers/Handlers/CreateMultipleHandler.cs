using System.Collections.Generic;
using MVCWarehousingSystem.Models;

namespace MVCWarehousingSystem.Controllers.Handlers
{
    internal class CreateMultipleHandler
    {
        public static List<StockItem> ItemsToBeAdded { get; set; }

        public static void Initiate()
        {
            if (ItemsToBeAdded == null)
                ItemsToBeAdded = new List<StockItem>();
            else
                ItemsToBeAdded.Clear();
        }

        public static void AddCreatedItem(StockItem item)
        {
            ItemsToBeAdded.Add(item);
        }
    }
}
