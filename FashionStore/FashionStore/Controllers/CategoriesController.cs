using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FashionStore.Models;

namespace FashionStore.Controllers
{
    public class CategoriesController : Controller
    {
        private ShopThoiTrangEntities db = new ShopThoiTrangEntities();

        // GET: Categories[ChildActionOnly]
        public ActionResult GetCategoryMenu()
        {
            var menu = db.CategoryGroups
                .Where(g => g.IsActive == true)
                .OrderBy(g => g.SortOrder)
                .Select(g => new CategoryMenuViewModel
                {
                    GroupId = g.GroupId,
                    GroupName = g.GroupName,
                    Categories = g.Categories
                        .Where(c => c.IsActive == true)
                        .OrderBy(c => c.SortOrder)
                        .Select(c => new CategoryItemViewModel
                        {
                            CategoryId = c.CategoryId,
                            CatName = c.CatName,
                            CatSlug = c.CatSlug
                        }).ToList()
                }).ToList();

            return PartialView("_CategoryMenu", menu);
        }

        [ChildActionOnly]
        public ActionResult GetNavMenu()
        {
            var model = db.CategoryGroups.Include("Categories").ToList();
            return PartialView("_NavMenu", model);
        }
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.CategoryGroup);
            return View(categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
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

        // GET: Categories/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.CategoryGroups, "GroupId", "GroupCode");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,GroupId,CatSlug,CatName,SortOrder,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupId = new SelectList(db.CategoryGroups, "GroupId", "GroupCode", category.GroupId);
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
            ViewBag.GroupId = new SelectList(db.CategoryGroups, "GroupId", "GroupCode", category.GroupId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,GroupId,CatSlug,CatName,SortOrder,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupId = new SelectList(db.CategoryGroups, "GroupId", "GroupCode", category.GroupId);
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
