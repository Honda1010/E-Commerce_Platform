using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;

namespace MyShop.Web.Controllers
{
    public class CategoryController1 : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController1(ApplicationDbContext context)
        {
          _context = context;
        }
        public IActionResult Index()
        {
            var category = _context.Categories.ToList();
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid) // for Validation
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                TempData["Create"] = "The item has been Created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) {
                NotFound();
            }
            var category_by_id = _context.Categories.Find(id);
            return View(category_by_id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid) // for Validation
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["Update"] = "The item has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            var category_by_id = _context.Categories.Find(id);
            return View(category_by_id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var category = _context.Categories.Find(id);
            if (id == null || id == 0)
            {
                NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["Delete"] = "Data has deleted Succesfully";
            return RedirectToAction("Index");
        }
    }
}
