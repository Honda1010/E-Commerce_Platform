using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity; // to represent identity of current user
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier); // find specific claim which contain user_id
            string User_Id = claim.Value; // return value of id from claim
            return View(_context.ApplicationUsers.Where( u => u.Id != User_Id));
        }
        public IActionResult LockUnlock(string? id) {
             var user = _context.ApplicationUsers.FirstOrDefault( u => u.Id == id);
            if (user == null) {
                NotFound();
            }
            if (user.LockoutEnd==null | user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
