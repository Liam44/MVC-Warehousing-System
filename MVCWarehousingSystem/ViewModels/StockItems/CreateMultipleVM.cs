using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public class CreateMultipleVM
    {
        public string Title { get; set; }
        public string ActionName { get; set; }
        public IEnumerable<StockItem> Items { get; set; }
    }
}