using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repository;
using MyShop.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MyShop.DataAccess.Implementations;

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
        public IActionResult Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                Product=_UnitOfWork.Product.GetFirstorDefault(p => p.Id == ProductId, Includedword:"Category"),
                ProductId= ProductId,
                Count=1
            };
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity; // to represent identity of current user
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier); // find specific claim which contain user_id
            shoppingCart.ApplicationUserId= claim.Value; // return value of id from claim
            ShoppingCart cartobj = _UnitOfWork.ShoppingCart.GetFirstorDefault(c=> c.ApplicationUserId==claim.Value && c.ProductId==shoppingCart.ProductId);
            if (cartobj == null)
            {
                _UnitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else {
                _UnitOfWork.ShoppingCart.IncreaseCount(cartobj,shoppingCart.Count);
            }
            _UnitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
