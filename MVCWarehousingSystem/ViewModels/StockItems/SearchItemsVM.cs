using MVCWarehousingSystem.Models;
using System.Collections.Generic;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public enum EViewType
    {
        Undefined,
        SearchByName,
        SearchByPrice,
        SearchByArticleNumber
    }

    public class SearchItemsVM
    {
        public string Value { get; set; }
        public EViewType ViewType { get; set; }
        public IEnumerable<StockItem> Result { get; set; }
    }

}