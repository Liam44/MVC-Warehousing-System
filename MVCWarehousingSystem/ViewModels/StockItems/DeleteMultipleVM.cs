using MVCWarehousingSystem.Models;
using System.Collections.Generic;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public class DeleteMultipleVM
    {
        public int[] ItemsToBeDeleted { get; set; }
        public List<StockItem> Items { get; set; }

        public void Initilize(List<StockItem> items)
        {
            Items = items;
            ItemsToBeDeleted = new int[] { };
        }
    }
}