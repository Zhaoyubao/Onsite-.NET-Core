using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private MysqlContext db;
        public HomeController(MysqlContext context)
        {
            db = context;
        }
       
// ================================================================================

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Products = db.Products.OrderByDescending(p => p.CreatedAt).ToList().Take(5);
            ViewBag.Users = db.Users.OrderByDescending(u => u.CreatedAt).ToList().Take(3);
            ViewBag.Orders = db.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.User)
                .Include(o => o.Product)
                .ToList()
                .Take(3);
            return View();
        }
// ================================================================================

        [HttpGet("customers")]
        public IActionResult Customers()
        {
            ViewBag.Users = db.Users
                .OrderByDescending(u => u.CreatedAt);
            return View();
        }

        [HttpPost("customers")]
        public IActionResult AddCustomer(User user)
        {
            if(ModelState.IsValid)
            {
                if(db.Users.SingleOrDefault(u => u.Name == user.Name.ToLower()) == null)
                {
                    user.Name = user.Name.ToLower();
                    db.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Customers");
                }
                ViewBag.Err = "Customer name already exist!";
            }
            ViewBag.Users = db.Users
                .OrderByDescending(u => u.CreatedAt);
            return View("customers");
        }
        
        [HttpGet("customers/{id}")]
        public IActionResult ShowCustomer(int id)
        {
            ViewBag.User = db.Users
                .Include( u => u.Orders )
                    .ThenInclude( o => o.Product )
                    .SingleOrDefault( u => u.UserId == id );
            return View("_order");
        }

        [HttpGet("customers/delete/{id}")]
        public IActionResult DelCustomer(int id)
        {
            User user = db.Users.Find(id);
            var orders = db.Orders.Where(o => o.UserId == user.UserId);
            db.Orders.RemoveRange(orders);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Customers");
        }

        [HttpPost("customers/filter")]
        public IActionResult CustomerFilter(string customer)
        {
            if(customer == null) customer = "";
            ViewBag.Users = db.Users
                .Where(u => u.Name.IndexOf(customer.ToLower()) >= 0)
                .OrderByDescending(u => u.CreatedAt);
            return View("_customers");
        }
// ================================================================================

        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.Products = db.Products
                .OrderByDescending(p => p.CreatedAt);
            return View();
        }

        [HttpPost("products")]
        public IActionResult AddProduct(Product prod)
        {
            if(ModelState.IsValid)
            {
                if(prod.ImgUrl == null) 
                    prod.ImgUrl = "http://www.trustvets.com/images/NoImageAvailable.png";
                db.Add(prod);
                db.SaveChanges();
                return RedirectToAction("Products");
            }
            ViewBag.Products = db.Products
                .OrderByDescending(p => p.CreatedAt);
            return View("products");
        }

        [HttpGet("products/delete/{id}")]
        public IActionResult DelProduct(int id)
        {
            Product prod = db.Products.Find(id);
            db.Products.Remove(prod);
            db.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpPost("products/filter")]
        public IActionResult ProductFilter(string product)
        {
            if(product == null) product = "";
            ViewBag.Products = db.Products
                .Where(p => p.Name.ToLower().IndexOf(product.ToLower()) >= 0)
                .OrderByDescending(p => p.CreatedAt);
            return View("_products");
        }
// ================================================================================

        [HttpGet("orders")]
        public IActionResult Orders()
        {
            ViewBag.Users = db.Users.OrderBy(u => u.Name);
            ViewBag.Products = db.Products.OrderBy(p => p.Name);
            ViewBag.Orders = db.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.User)
                .Include(o => o.Product);
            ViewBag.Err = TempData["Err"];
            return View();
        }

        [HttpPost("orders")]
        public IActionResult AddOrder(Order order)
        {
            Product prod = db.Products.Find(order.ProductId);
            if(order.Quantity > prod.Quantity)
            {
                TempData["Err"] = "Exceed the max quantity!";
                return RedirectToAction("Orders");
            }
            db.Add(order);
            prod.Quantity -= order.Quantity;
            db.SaveChanges();
            return RedirectToAction("Orders");
        }

        [HttpGet("orders/delete/{id}")]
        public IActionResult DelOrder(int id)
        {
            Order order = db.Orders.Find(id);
            Product prod = db.Products.Find(order.ProductId);
            prod.Quantity += order.Quantity;
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Orders");
        }

    }
}