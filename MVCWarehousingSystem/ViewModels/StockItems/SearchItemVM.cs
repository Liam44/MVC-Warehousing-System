using MVCWarehousingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public enum eViewType
    {
        Undefined,
        SearchByName,
        SearchByPrice,
        SearchByArticleNumber
    }

    public class SearchItemsVM
    {
        public string Value { get; set; }
        public eViewType ViewType { get; set; }
        public IEnumerable<StockItem> Result { get; set; }
    }
}