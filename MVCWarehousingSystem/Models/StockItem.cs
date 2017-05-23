using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCWarehousingSystem.Models
{
    public class StockItem
    {
        [Key]
        public int ArticleNumber { get; set; }

        [Display(Name = "Article name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        
        [Display(Name = "Position on shelf")]
        public string ShelfPosition { get; set; }

        [Display(Name = "Quantity available")]
        public int Quantity { get; set; }

        public string Description { get; set; }
    }
}