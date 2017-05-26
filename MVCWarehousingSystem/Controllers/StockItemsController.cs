using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCWarehousingSystem.Models;
using MVCWarehousingSystem.Repositories;
using MVCWarehousingSystem.ViewModels.StockItems;
using System.Globalization;
using System.Web.Routing;
using System.IO;

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

        // GET: StockItems/CreateMultiple
        public ActionResult CreateMultiple()
        {
            StaticItemList.Items = new List<StockItem>();
            //CreateMultipleHandler.Initiate();
            return View();
        }

        // POST: StockItems/CreateMultiple
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMultiple([Bind(Include = "ArticleNumber,Name,Price,ShelfPosition,Quantity,Description")] StockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                StaticItemList.Items.Add(stockItem);

                ModelState.Clear();
                return View();
            }

            return View(stockItem);
        }

        public ActionResult ValidateCreateMultiple()
        {
            if (StaticItemList.Items.Count == 0)
                return RedirectToAction("Index");

            sir.AddItems(StaticItemList.Items);
            return View(StaticItemList.Items);
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeleteMultiple()
        {
            ModelState.Clear();
            DeleteMultipleVM deleteVM = new DeleteMultipleVM();
            deleteVM.Initilize(sir.Items);
            return View(deleteVM);
        }

        [HttpPost]
        public ActionResult DeleteMultiple(DeleteMultipleVM model)
        {
            if (model.ItemsToBeDeleted == null)
                return RedirectToAction("Index");

            List<StockItem> itemsToBeDeleted = new List<StockItem>();

            foreach (int articleNumber in model.ItemsToBeDeleted)
            {
                itemsToBeDeleted.Add(sir.ItemByArticleNumber(articleNumber));
            }

            sir.DeleteItems(itemsToBeDeleted);
            StaticItemList.Items = itemsToBeDeleted;
            return RedirectToAction("DeleteMultipleConfirmed");
        }

        public ActionResult DeleteMultipleConfirmed()
        {
            return View(StaticItemList.Items);
        }

        #endregion

        public ActionResult Search(string searchedValue, string sortOrder)
        {
            try
            {
                int id = 0;
                double price = 0;
                IEnumerable<StockItem> result = null;
                eViewType viewType = eViewType.Undefined;

                if (int.TryParse(searchedValue, out id))
                {
                    viewType = eViewType.SearchByArticleNumber;
                    result = new List<StockItem> { sir.ItemByArticleNumber(id) };
                }
                else
                {
                    string currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol.ToLower();

                    if (double.TryParse(searchedValue.ToLower().Replace(currency,
                                                                        string.Empty).Trim(),
                                        out price))
                    {
                        viewType = eViewType.SearchByPrice;
                        result = sir.ItemsByPrice(price);
                        searchedValue = searchedValue.Trim();
                    }
                    else
                    {
                        viewType = eViewType.SearchByName;
                        result = sir.ItemsByName(searchedValue);
                    }
                }

                if (result.Count() == 0)
                    return View("NoArticleFound");
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

        // This action renders the form
        public ActionResult ImportArticles()
        {
            return View();
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult ImportArticles(ImportArticlesVM viewModel)
        {
            List<StockItem> importedArticles = new List<StockItem>();

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

            // extract only the filename
            var fileName = Path.GetFileName(Request.Files[0].FileName);

            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

            try
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                Request.Files[0].SaveAs(path);
            }
            catch (Exception)
            {
                ModelState.AddModelError("uploadError", "Can't save file to disk");
            }

            if (ModelState.IsValid)
            {
                // put your logic here

                return View("ImportedArticles", new { viewModel = importedArticles });
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
