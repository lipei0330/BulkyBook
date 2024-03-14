using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.ProductRepo.Add(product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            return View(product);
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
