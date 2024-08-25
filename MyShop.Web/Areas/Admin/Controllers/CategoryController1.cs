using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.Implementations;
using MyShop.Entities.Models;
using MyShop.Entities.Repository;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController1 : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IUnitOfWork _UnitOfWork;
        public CategoryController1(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            //var category = _context.Categories.ToList();
            var category = _UnitOfWork.Category.GetAll();

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
                //_context.Categories.Add(category);
                _UnitOfWork.Category.Add(category);
                //_context.SaveChanges();
                _UnitOfWork.Complete();
                TempData["Create"] = "The item has been Created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            //var category_by_id = _context.Categories.Find(id);
            var category_by_id = _UnitOfWork.Category.GetFirstorDefault(c => c.Id == id);
            return View(category_by_id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid) // for Validation
            {
                //_context.Categories.Update(category);
                _UnitOfWork.Category.Update(category);
                //_context.SaveChanges();
                _UnitOfWork.Complete();
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
            //var category_by_id = _context.Categories.Find(id);
            var category_by_id = _UnitOfWork.Category.GetFirstorDefault(c => c.Id == id);
            return View(category_by_id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            //var category = _context.Categories.Find(id);
            var category = _UnitOfWork.Category.GetFirstorDefault(c => c.Id == id);
            if (id == null || id == 0)
            {
                NotFound();
            }
            //_context.Categories.Remove(category);
            _UnitOfWork.Category.Remove(category);
            //_context.SaveChanges();
            _UnitOfWork.Complete();
            TempData["Delete"] = "Data has deleted Succesfully";
            return RedirectToAction("Index");
        }
    }
}
