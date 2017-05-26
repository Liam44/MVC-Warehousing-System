using MVCWarehousingSystem.Controllers.Handlers;
using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public class DeleteMultipleVM
    {
        public Dictionary<StockItem, bool> ItemsToBeDeleted
        {
            get
            {
                return DeleteMultipleHandler.ItemsToBeDeleted;
            }
            private set { }
        }

        public void Initiate(IEnumerable<StockItem> items)
        {
            DeleteMultipleHandler.Initiate(items);
        }
    }
}