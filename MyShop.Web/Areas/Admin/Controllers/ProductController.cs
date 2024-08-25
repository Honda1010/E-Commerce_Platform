using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.Implementations;
using MyShop.Entities.Models;
using MyShop.Entities.Repository;
using MyShop.Entities.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //var product = _context.Categories.ToList();
            //var product = _UnitOfWork.Product.GetAll(null,"Category");
            return View();
        }
        [HttpGet]
        public IActionResult Getdata()
        {
            var Product =_UnitOfWork.Product.GetAll(Includedword:"Category");
            return Json(new {data = Product});
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(), // create new product
                Categorylist = _UnitOfWork.Category.GetAll().Select(x => new SelectListItem 
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
                //select specific category for new product
            }; 
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid) // for Validation
            {
                string root = _WebHostEnvironment.WebRootPath; // path of wwwroot
                if (file != null)
                {
                    string Filename= Guid.NewGuid().ToString(); // random filename
                    var Upload= Path.Combine(root, @"Images\Products"); // give him specific file
                    var ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(Upload, Filename+ext),FileMode.Create)) 
                    {
                            file.CopyTo(filestream);
                        // to transfer file from input to specific file and with randam name 
                    }
                    productVM.Product.Image = @"Images\Products\" + Filename + ext; // store in database
                }
                _UnitOfWork.Product.Add(productVM.Product);
                _UnitOfWork.Complete();
                TempData["Create"] = "The item has been Created successfully";
                return RedirectToAction("Index");
            }
            return View(productVM);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();
            }
            ProductVM productVM = new ProductVM()
            {
                Product = _UnitOfWork.Product.GetFirstorDefault(c => c.Id == id), // get element by id from database
                Categorylist = _UnitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM , IFormFile? file)
        {
            if (ModelState.IsValid) // for Validation
            {
                string root = _WebHostEnvironment.WebRootPath; // path of wwwroot
                if (file != null)
                {
                    string Filename = Guid.NewGuid().ToString(); // random filename
                    var Upload = Path.Combine(root, @"Images\Products"); // give him specific file
                    var ext = Path.GetExtension(file.FileName);
                    if (productVM.Product.Image != null)
                    {
                        var Oldimg=Path.Combine(root,productVM.Product.Image.Trim('\\')); // path of old image
                        if (System.IO.File.Exists(Oldimg))
                        {
                            System.IO.File.Delete(Oldimg);

                        }

                    }
                    using (var filestream = new FileStream(Path.Combine(Upload, Filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                        // to transfer file from input to specific file and with randam name 
                    }
                    productVM.Product.Image = @"Images\Products\" + Filename + ext; // store in database
                }
                _UnitOfWork.Product.Update(productVM.Product);
                _UnitOfWork.Complete();
                TempData["Update"] = "The item has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }
        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {
            var product_By_Id = _UnitOfWork.Product.GetFirstorDefault(c => c.Id == id);
            if (product_By_Id==null)
            {
                return Json(new { success = false, message = "Error Occurs During Delete" });
            }
            _UnitOfWork.Product.Remove(product_By_Id);
            var Oldimg = Path.Combine(_WebHostEnvironment.WebRootPath, product_By_Id.Image.Trim('\\')); // path of old image
            if (System.IO.File.Exists(Oldimg))
            {
                System.IO.File.Delete(Oldimg); // delete old image from folder

            }
            _UnitOfWork.Complete();
            //TempData["Delete"] = "Data has deleted Succesfully";
            return Json(new { success = true, message = "File has been deleted successfully" });
        }
    }
}
