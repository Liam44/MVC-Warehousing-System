using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace MVCWarehousingSystem.Models
{
    public class StockItem
    {
        [Key]
        [Display(Name = "ID")]
        [XmlIgnore]
        public int ArticleNumber { get; set; }

        [Display(Name = "Article name")]
        [XmlAttribute("articlename")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        [XmlAttribute("price")]
        public double Price { get; set; }
        
        [Display(Name = "Position on shelf")]
        [XmlAttribute("shelfposition")]
        public string ShelfPosition { get; set; }

        [Display(Name = "Quantity available")]
        [XmlAttribute("quantity")]
        public int Quantity { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }
    }
}