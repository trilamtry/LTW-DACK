using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FashionStore.Areas.Admin.Models;
using FashionStore.Models;

namespace FashionStore.Areas.Admin.Controllers
{
    public class ProductVariantController : Controller
    {
        private FashionStoreContext db = new FashionStoreContext();

        // 1. Danh sách các biến thể của 1 sản phẩm cụ thể
        // GET: Admin/ProductVariant/Index?productId=5
        public ActionResult Index(int? productId)
        {
            if (productId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            ViewBag.Product = product; // Truyền thông tin SP gốc sang View

            var variants = db.ProductVariants
                             .Include(v => v.Size)
                             .Include(v => v.Color)
                             .Where(v => v.ProductId == productId)
                             .OrderBy(v => v.Size.SortOrder)
                             .ToList();
            return View(variants);
        }

        // 2. Thêm mới biến thể
        // GET: Admin/ProductVariant/Create?productId=5
        public ActionResult Create(int? productId)
        {
            if (productId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Find(productId);
            if (product == null) return HttpNotFound();

            // Chuẩn bị dữ liệu cho Form
            var model = new ProductVariantViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.BasePrice, // Mặc định lấy giá gốc
                Stock = 0
            };

            ViewBag.SizeId = new SelectList(db.Sizes.OrderBy(s => s.SortOrder), "SizeId", "SizeCode");
            ViewBag.ColorId = new SelectList(db.Colors, "ColorId", "ColorName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVariantViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem biến thể này đã tồn tại chưa (Trùng Size + Màu)
                bool exists = db.ProductVariants.Any(v => v.ProductId == model.ProductId
                                                       && v.SizeId == model.SizeId
                                                       && v.ColorId == model.ColorId);
                if (exists)
                {
                    ModelState.AddModelError("", "Biến thể này (Size + Màu) đã tồn tại!");
                }
                else
                {
                    var variant = new ProductVariant
                    {
                        ProductId = model.ProductId,
                        SizeId = model.SizeId,
                        ColorId = model.ColorId,
                        Price = model.Price,
                        Stock = model.Stock,
                        // Tạo SKU tự động: SP001-M-RED
                        SKU = $"{model.ProductId}-{model.SizeId}-{model.ColorId}"
                    };

                    db.ProductVariants.Add(variant);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { productId = model.ProductId });
                }
            }

            ViewBag.SizeId = new SelectList(db.Sizes.OrderBy(s => s.SortOrder), "SizeId", "SizeCode", model.SizeId);
            ViewBag.ColorId = new SelectList(db.Colors, "ColorId", "ColorName", model.ColorId);
            return View(model);
        }

        // 3. Xóa biến thể
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var variant = db.ProductVariants.Find(id);
            int productId = variant.ProductId; // Lưu lại để redirect
            if (variant != null)
            {
                db.ProductVariants.Remove(variant);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { productId = productId });
        }
    }
}