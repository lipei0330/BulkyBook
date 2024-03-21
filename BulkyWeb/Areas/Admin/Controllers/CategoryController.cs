using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unit;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unit.CategoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DispalyOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The Display Order cannot be exactly macth the Name!");
            //}

            if (ModelState.IsValid)
            {
                _unit.CategoryRepo.Add(obj);
                _unit.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = _unit.CategoryRepo.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unit.CategoryRepo.Update(obj);
                _unit.Save();
                TempData["success"] = "Category edited successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category category = _unit.CategoryRepo.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category obj = _unit.CategoryRepo.Get(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unit.CategoryRepo.Remove(obj);
            TempData["success"] = "Category deleted successfully!";
            _unit.Save();
            return RedirectToAction("Index");
        }
    }
}
