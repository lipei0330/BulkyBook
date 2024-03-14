using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        //[BindProperty]
        public Category Category { get; set; }
        public CreateModel(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category created successfully!";
            return RedirectToPage("Index");
        }

    }
}
