using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NorthwindTest;
using Newtonsoft.Json;

namespace NorthwindTest.Controllers
{
    public class CategoriesController : Controller
    {
        private OldNorthwindEntities db = new OldNorthwindEntities();

        // GET: Categories
        public ActionResult Index()
        {
            //var catData = db.Categories.Include(c => c.Products);
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category = db.Categories.Find(id);
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category); ;
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryKey,CategoryName,Description,Picture")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryKey,CategoryName,Description,Picture")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private static List<DataPoint> _dataPoints;
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };

        public ActionResult categoryCharts(string chartType)
        {
            if(chartType is null || chartType.Length<3)
            {
                chartType = "pie";
            }
            _dataPoints = new List<DataPoint>();
            var categories = db.Categories.Where(c => c.Products.Count > 0);
            try
            {
                foreach (var cat in categories)
                {
                    var x = cat.Products.Count();
                    var y = cat.CategoryName;
                    _dataPoints.Add(new DataPoint(x, y));
                }
                ViewBag.chartType = chartType;
                ViewBag.chartTitle = "Number of Products in Northwind Categories"; 
                ViewBag.DataPoints = JsonConvert.SerializeObject(_dataPoints.ToList(), _jsonSetting);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }
            return View();
        }

        public ActionResult customerCharts(string chartType)
        {
            if (chartType is null || chartType.Length < 3)
            {
                chartType = "pie";
            }
            _dataPoints = new List<DataPoint>();
            var customer = db.CustomerTotals.OrderByDescending(t => t.TotalSpent).Take(20);
            try
            {
                foreach (var company in customer)
                {
                    var x = Convert.ToInt32(company.TotalSpent);  // the version of DataPoint used requires an integer value
                    var y = company.CompanyName;
                    _dataPoints.Add(new DataPoint(x, y));
                }
                ViewBag.chartType = chartType;
                ViewBag.chartTitle = "20 Top Spending Companies";
                ViewBag.DataPoints = JsonConvert.SerializeObject(_dataPoints.ToList(), _jsonSetting);
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
