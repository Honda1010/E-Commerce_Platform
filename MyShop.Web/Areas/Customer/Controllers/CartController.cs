using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Repository;
using MyShop.Entities.ViewModels;
using System.Security.Claims;


namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int TotalCarts { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity; // to represent identity of current user
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier); // find specific claim which contain user_id
            ShoppingCartVM = new ShoppingCartVM()
            {
                CartList = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId==claim.Value, Includedword:"Product")
            };
            foreach (var item in ShoppingCartVM.CartList)
            {
                ShoppingCartVM.TotalCarts += (item.Count * item.Product.price);

			}
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int CartId) {
            var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(c => c.Id == CartId);
            _unitOfWork.ShoppingCart.IncreaseCount(shoppingcart,1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
		public IActionResult Minus(int CartId)
		{
			var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(c => c.Id == CartId);
            if (shoppingcart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingcart);
            }
            else
            {
            _unitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
			}
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}
        public IActionResult remove(int CartId) {
			var shoppingcart = _unitOfWork.ShoppingCart.GetFirstorDefault(c => c.Id == CartId);
			_unitOfWork.ShoppingCart.Remove(shoppingcart);
			_unitOfWork.Complete();
			return RedirectToAction("Index");
		}
	}
}
