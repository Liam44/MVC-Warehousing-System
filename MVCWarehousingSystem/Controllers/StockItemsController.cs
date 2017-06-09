using MVCWarehousingSystem.Models;
using MVCWarehousingSystem.Repositories;
using MVCWarehousingSystem.ViewModels.StockItems;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MVCWarehousingSystem.Controllers
{
    public class StockItemsController : Controller
    {
        StockItemRepository sir = new StockItemRepository();

        private IEnumerable<StockItem> Sort(IEnumerable<StockItem> list, string sortOrder)
        {
            ViewBag.ArticleNumberSortParam = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.PriceSortParam = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.LocationSortParam = sortOrder == "location_asc" ? "location_desc" : "location_asc";
            ViewBag.QuantitySortParam = sortOrder == "quantity_asc" ? "quantity_desc" : "quantity_asc";
            ViewBag.DescriptionSortParam = sortOrder == "description_asc" ? "description_desc" : "description_asc";

            switch (sortOrder)
            {
                case "number_desc":
                    list = list.OrderByDescending(i => i.ArticleNumber);
                    break;
                case "name_asc":
                    list = list.OrderBy(i => i.Name);
                    break;
                case "name_desc":
                    list = list.OrderByDescending(i => i.Name);
                    break;
                case "price_asc":
                    list = list.OrderBy(i => i.Price);
                    break;
                case "price_desc":
                    list = list.OrderByDescending(i => i.Price);
                    break;
                case "location_asc":
                    list = list.OrderBy(i => i.ShelfPosition);
                    break;
                case "location_desc":
                    list = list.OrderByDescending(i => i.ShelfPosition);
                    break;
                case "quantity_asc":
                    list = list.OrderBy(i => i.Quantity);
                    break;
                case "quantity_desc":
                    list = list.OrderByDescending(i => i.Quantity);
                    break;
                case "description_asc":
                    list = list.OrderBy(i => i.Description);
                    break;
                case "description_desc":
                    list = list.OrderByDescending(i => i.Description);
                    break;
                default:
                    list = list.OrderBy(i => i.ArticleNumber);
                    break;
            }

            return list;
        }

        // GET: StockItems
        public ActionResult Index(string sortOrder)
        {
            return View(Sort(sir.Items, sortOrder).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "About My Store.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "If you have any questions, feel free to visit us, phone us or send us an e-mail.";

            return View();
        }

        public ActionResult BackToList()
        {
            StaticItemList.Items = new List<StockItem>();
            return RedirectToAction("Index");
        }

        // GET: StockItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = sir.ItemByArticleNumber(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        #region Single and multiple articles creation

        // GET: StockItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StockItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleNumber,Name,Price,ShelfPosition,Quantity,Description")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                sir.AddItem(stockItem);
                return RedirectToAction("Index");
            }

            return View(stockItem);
        }

        [HttpGet]
        public ActionResult CreateMultiple()
        {
            StaticItemList.Items = new List<StockItem>();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMultiple([Bind(Include = "ArticleNumber,Name,Price,ShelfPosition,Quantity,Description")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                // Stock the created article in a static list of articles
                StaticItemList.Items.Add(stockItem);

                // Loops on the view until the user clicks on 'Validate'
                ModelState.Clear();
                return View();
            }

            // Something went wrong during the validation of the data:
            // The article under creation is displayed again
            return View(stockItem);
        }

        public ActionResult ValidateCreateMultiple()
        {
            // If the user didn't create any articles before clicking on 'Validate'
            if (StaticItemList.Items.Count == 0)
                // Redirect to 'Index'
                return RedirectToAction("Index");

            // Mass creation of the articles
            sir.AddItems(StaticItemList.Items);

            // Reinitiate the static list of items
            List<StockItem> tmp = StaticItemList.Items.ToList();
            StaticItemList.Items = new List<StockItem>();

            // Displays the list of created articles
            return View(new CreateMultipleVM { Title = "Multuple Articles created", ActionName = "created", Items = tmp });
        }

        #endregion

        // GET: StockItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = sir.ItemByArticleNumber(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // POST: StockItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleNumber,Name,Price,ShelfPosition,Quantity,Description")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                sir.ChangeItemState(stockItem, EntityState.Modified);
                return RedirectToAction("Index");
            }
            return View(stockItem);
        }

        #region Single and multiple articles delation

        #region Single article delation

        // GET: StockItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockItem stockItem = sir.ItemByArticleNumber(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // POST: StockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockItem stockItem = sir.ItemByArticleNumber(id);
            sir.DeleteItem(stockItem);
            return RedirectToAction("Index");
        }

        #endregion

        #region Multiple articles delation

        [HttpGet]
        public ActionResult DeleteMultiple()
        {
            if (StaticItemList.Items == null)
                StaticItemList.Items = new List<StockItem>();

            // Recreation of the model
            DeleteMultipleVM deleteVM = new DeleteMultipleVM();
            deleteVM.Initilize(sir.Items);

            deleteVM.ItemsToBeDeleted = (StaticItemList.Items.Select(i => i.ArticleNumber)).ToArray();

            // Displays the list of articles
            return View(deleteVM);
        }

        [HttpGet]
        public ActionResult DeleteMultipleConfirm()
        {
            // This happens if the page is directly accessed via the URL
            // Redirection to 'Index', instead of generating an error
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteMultipleConfirm(DeleteMultipleVM model)
        {
            // The user clicked on the button without having selected any articles
            if (model.ItemsToBeDeleted == null)
                return RedirectToAction("Index");

            // Reconstruction of the model
            DeleteMultipleVM deleteVM = new DeleteMultipleVM();
            deleteVM.Initilize(sir.Items);

            if (model.ItemsToBeDeleted != null)
                deleteVM.ItemsToBeDeleted = model.ItemsToBeDeleted;

            // Reconstruction of the list of articles needed to be deleted
            StaticItemList.Items = new List<StockItem>();

            foreach (int articleNumber in model.ItemsToBeDeleted)
                StaticItemList.Items.Add(sir.ItemByArticleNumber(articleNumber));

            // Displays the confirmation of delation
            return View(deleteVM);
        }

        public ActionResult DeleteMultipleConfirmed()
        {
            // Physically delete the selected articles
            sir.DeleteItems(StaticItemList.Items);

            // Reinitiate the static list of items
            List<StockItem> tmp = StaticItemList.Items.ToList();
            StaticItemList.Items = new List<StockItem>();

            // Displays the list of created articles
            return View(tmp);
        }

        #endregion

        #endregion

        public ActionResult Search(string searchedValue, string sortOrder)
        {
            try
            {
                int id = 0;
                double price = 0;
                IEnumerable<StockItem> result = null;
                EViewType viewType = EViewType.Undefined;

                if (int.TryParse(searchedValue, out id))
                {
                    viewType = EViewType.SearchByArticleNumber;
                    StockItem item = sir.ItemByArticleNumber(id);

                    if (item == null)
                        result = new List<StockItem>();
                    else
                        result = new List<StockItem> { item };
                }
                else
                {
                    string currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol.ToLower();

                    if (double.TryParse(searchedValue.ToLower().Replace(currency,
                                                                        string.Empty).Trim(),
                                        out price))
                    {
                        viewType = EViewType.SearchByPrice;
                        result = sir.ItemsByPrice(price);
                        searchedValue = searchedValue.Trim();
                    }
                    else
                    {
                        viewType = EViewType.SearchByName;
                        result = sir.ItemsByName(searchedValue.Replace("\"", string.Empty));
                    }
                }

                if (result.Count() == 0)
                    return View("NoArticleFound", "", searchedValue);
                else
                {
                    return View(new SearchItemsVM
                    {
                        Value = searchedValue,
                        Result = Sort(result, sortOrder),
                        ViewType = viewType
                    });
                }
            }
            catch
            {
                return View("NoArticleFound");
            }
        }

        #region XLM files management

        [HttpGet]
        public void Export()
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ExportedArticles.xml");
            Response.ContentType = "text/xml";

            List<StockItem> data = sir.Items;

            XmlSerializer serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(Response.OutputStream, data);
        }

        // This action renders the form
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Import(ImportArticlesVM viewModel)
        {
            // if file's content length is zero or no files submitted
            if (Request.Files.Count != 1 || Request.Files[0].ContentLength == 0)
            {
                ModelState.AddModelError("uploadError", "File's length is zero, or no files found");
                return View(viewModel);
            }

            // check the file size (max 4 Mb)
            if (Request.Files[0].ContentLength > 1024 * 1024 * 4)
            {
                ModelState.AddModelError("uploadError", "File size can't exceed 4 MB");
                return View(viewModel);
            }

            // check the file size (min 100 bytes)
            if (Request.Files[0].ContentLength < 100)
            {
                ModelState.AddModelError("uploadError", "File size is too small");
                return View(viewModel);
            }

            // check file extension
            string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();

            if (extension != ".xml")
            {
                ModelState.AddModelError("uploadError", "You may only upload XML files (xml extension).");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                XElement elements = XElement.Load(Request.Files[0].InputStream);

                List<StockItem> importedArticles = sir.Deserialize(elements);

                if (importedArticles == null)
                {
                    ModelState.AddModelError("uploadError", "Error in reading XML.");
                    return View(viewModel);
                }
                else
                    return View("ValidateCreateMultiple",
                                new CreateMultipleVM
                                {
                                    Title = "Imported Articles",
                                    ActionName = "imported",
                                    Items = importedArticles
                                });
            }

            return View(viewModel);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sir.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
