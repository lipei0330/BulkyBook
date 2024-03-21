using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepo.GetAll(includeProperties:"Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            /**
            //IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepo.GetAll().Select(
            //    c => new SelectListItem
            //    {
            //        Text = c.Name,
            //        Value = c.Id.ToString()
            //    }
            //    );
            //ViewBag passed data from controller to view
            //ViewBag.CategoryList = categoryList;
            **/

            ProductViewModel productViewModel = new ProductViewModel()
            {
                CategoryList = _unitOfWork.CategoryRepo.GetAll().Select(
                    c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }),

                Product = new Product()
            };
            //create
            if (id == null || id == 0)
            {
                return View(productViewModel);
            }
            else 
            {
                // update
                productViewModel.Product = _unitOfWork.ProductRepo.Get(p => p.Id == id);
                return View(productViewModel);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel prodVM, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                //upload image
                //get root path of wwwroot
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    //replace the file name with a guid one
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    //handle update -> if need to udpate imageUrl, means has old one
                    if (!string.IsNullOrEmpty(prodVM.Product.ImagUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, prodVM.Product.ImagUrl.TrimStart('\\'));
                        
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    
                    //save file into wwwRoot
                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    //save the image url to product view model
                    prodVM.Product.ImagUrl = @"\images\product\" + fileName;
                }

                //identity if update or create
                if(prodVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepo.Add(prodVM.Product);
                }
                else
                {
                    _unitOfWork.ProductRepo.Update(prodVM.Product);
                }
                
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                prodVM.CategoryList = _unitOfWork.CategoryRepo.GetAll().Select(
                c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
                return View(prodVM);
            }
            
        }

        #region Delete Func MVC
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
        //    if (prod == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(prod);
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
        //    if (prod == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.ProductRepo.Remove(prod);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product deleted successfully!";
        //    return RedirectToAction("Index");
        //}
        #endregion


        #region API CALLS
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.ProductRepo.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = products});

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product prod = _unitOfWork.ProductRepo.Get(p => p.Id == id);
            if (prod == null)
            {
                return Json(new {success=false, message="Error while deleting"});
            }

            //need to delete the image in wwwRoot
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                               prod.ImagUrl.TrimStart('\\'));
            
            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            //update database
            _unitOfWork.ProductRepo.Remove(prod);
            _unitOfWork.Save();

            return Json(new {success = true, message = "Delete Successful"});

        }
        #endregion
    }
}
