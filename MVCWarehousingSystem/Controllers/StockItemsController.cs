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
using MVCWarehousingSystem.Controllers.Handlers;
using System.Globalization;

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
            CreateMultipleHandler.Initiate();
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
                CreateMultipleHandler.AddCreatedItem(stockItem);

                ModelState.Clear();
                return View();
            }

            return View(stockItem);
        }

        public ActionResult ValidateCreateMultiple()
        {
            sir.AddItems(CreateMultipleHandler.ItemsToBeAdded);
            return View(CreateMultipleHandler.ItemsToBeAdded);
        }

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

        public ActionResult DeleteMultiple()
        {
            DeleteMultipleVM dmvm = new DeleteMultipleVM();

            dmvm.Initiate(sir.Items);

            return View(dmvm);
        }

        public ActionResult DeleteMultipleConfirmed(DeleteMultipleVM model)
        {
            List<StockItem> itemsToBeDeleted = new List<StockItem>();

            foreach (StockItem item in model.ItemsToBeDeleted.Keys.ToList())
            {
                if (model.ItemsToBeDeleted[item])
                {
                    itemsToBeDeleted.Add(item);
                }
                else
                {
                    model.ItemsToBeDeleted.Remove(item);
                }
            }

            sir.DeleteItems(itemsToBeDeleted);
            return View(model);
        }

        public ActionResult Search(string searchedValue, string sortOrder)
        {
            try
            {
                IEnumerable<StockItem> result = null;
                eViewType viewType = eViewType.Undefined;

                if (int.TryParse(searchedValue, out int id))
                {
                    viewType = eViewType.SearchByArticleNumber;
                    result = new List<StockItem> { sir.ItemByArticleNumber(id) };
                }
                else
                {
                    string currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol.ToLower();

                    if (double.TryParse(searchedValue.ToLower().Replace(currency, string.Empty).Trim(),
                                        out double price))
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
