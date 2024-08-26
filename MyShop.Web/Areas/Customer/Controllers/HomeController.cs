using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repository;
using MyShop.Entities.ViewModels;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public HomeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            var product = _UnitOfWork.Product.GetAll();
            return View(product);
        }
        public IActionResult Details(int? id) {

            var ShoppingCart = new ShoppingCart()
            {
                Product = _UnitOfWork.Product.GetFirstorDefault(p => p.Id == id, Includedword: "Category"),
                Count=1
            };
            return View(ShoppingCart);
        }
    }
}
