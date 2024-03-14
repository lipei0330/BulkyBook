using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
                _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> products= _unitOfWork.ProductRepo.GetAll().ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepo.GetAll().Select(
            //    c => new SelectListItem
            //    {
            //        Text = c.Name,
            //        Value = c.Id.ToString()
            //    }
            //    );
            //ViewBag passed data from controller to view
            //ViewBag.CategoryList = categoryList;

            ProductViewModel prodVM = new ProductViewModel()
            {
                CategoryList = _unitOfWork.CategoryRepo.GetAll().Select(
                    c=> new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),

                Product = new Product()
            };
            return View(prodVM);
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel prodVM)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.ProductRepo.Add(prodVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            return View(prodVM);
        }

        public IActionResult Edit(int id) 
        { 
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
            if(prod == null)
            {
                return NotFound();
            }
            return View(prod);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepo.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product edited successfully!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
            if(prod == null)
            {
                return NotFound() ;
            }
            return View(prod);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
            if(prod == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepo.Remove(prod);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
