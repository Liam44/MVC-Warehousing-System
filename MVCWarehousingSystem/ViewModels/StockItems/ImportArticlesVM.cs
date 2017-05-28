using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MVCWarehousingSystem.ViewModels.StockItems
{
    public class ImportArticlesVM
    {
        public HttpPostedFileBase File { get; set; }

        [Required, FileExtensions(Extensions = "xml", ErrorMessage = "Specify a XML file. (Comma-separated values)")]
        public string FileName
        {
            get
            {
                if (File == null)
                    return String.Empty;
                else
                    return File.FileName;
            }
            private set { }
        }
    }
}