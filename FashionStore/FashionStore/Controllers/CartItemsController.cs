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
    public class CartItemsController : Controller
    {
        private ShopThoiTrangEntities db = new ShopThoiTrangEntities();
        [HttpPost]
        public ActionResult AddToCart(int variantId, int? quantity)
        {
            Cart cart;

            if (Session["user"] != null) // đã đăng nhập
            {
                var user = (AppUser)Session["user"];
                cart = db.Carts.FirstOrDefault(c => c.UserId == user.UserId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        CartToken = Guid.NewGuid().ToString(),
                        UserId = user.UserId,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
            }
            else // chưa đăng nhập → giỏ hàng tạm
            {
                string cartToken = Request.Cookies["CartToken"]?.Value;
                if (string.IsNullOrEmpty(cartToken))
                {
                    cartToken = Guid.NewGuid().ToString();
                    Response.Cookies.Add(new HttpCookie("CartToken", cartToken)
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                }

                cart = db.Carts.FirstOrDefault(c => c.CartToken == cartToken);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CartToken = cartToken,
                        UserId = null,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
            }



            // thêm sản phẩm vào CartItem
            var variant = db.ProductVariants.Find(variantId);
            if (variant == null) return HttpNotFound();

            var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.VariantId == variantId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    VariantId = variantId,
                    Quantity = quantity,
                    UnitPrice = variant.Price
                };
                db.CartItems.Add(cartItem);
            }

            cart.UpdatedAt = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int cartItemId)
        {
            CartItem cartItem = null;

            if (Session["user"] != null) // đã đăng nhập
            {
                var user = (AppUser)Session["user"];
                cartItem = db.CartItems
                    .FirstOrDefault(ci => ci.CartItemId == cartItemId && ci.Cart.UserId == user.UserId);
            }
            else // chưa đăng nhập → giỏ hàng tạm
            {
                string cartToken = Request.Cookies["CartToken"]?.Value;
                if (!string.IsNullOrEmpty(cartToken))
                {
                    cartItem = db.CartItems
                        .FirstOrDefault(ci => ci.CartItemId == cartItemId && ci.Cart.CartToken == cartToken);
                }
            }

            if (cartItem != null)
            {
                db.CartItems.Remove(cartItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        // GET: CartItems
        public ActionResult Index()
        {
            var cartItems = db.CartItems.Include(c => c.Cart).Include(c => c.ProductVariant);
            return View(cartItems.ToList());
        }

        // GET: CartItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // GET: CartItems/Create
        public ActionResult Create()
        {
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartToken");
            ViewBag.VariantId = new SelectList(db.ProductVariants, "VariantId", "SKU");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CartItemId,CartId,VariantId,Quantity,UnitPrice")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.CartItems.Add(cartItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartToken", cartItem.CartId);
            ViewBag.VariantId = new SelectList(db.ProductVariants, "VariantId", "SKU", cartItem.VariantId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartToken", cartItem.CartId);
            ViewBag.VariantId = new SelectList(db.ProductVariants, "VariantId", "SKU", cartItem.VariantId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartItemId,CartId,VariantId,Quantity,UnitPrice")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.Carts, "CartId", "CartToken", cartItem.CartId);
            ViewBag.VariantId = new SelectList(db.ProductVariants, "VariantId", "SKU", cartItem.VariantId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
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
