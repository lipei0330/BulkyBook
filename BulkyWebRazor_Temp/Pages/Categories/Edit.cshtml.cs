using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        public Category Category { get; set; }
        public EditModel(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }
        public void OnGet(int? id)
        {
            if(id != null || id != 0)
            {
                Category = _db.Categories.Find(id);
            }
            
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _db.Categories.Update(Category);
            _db.SaveChanges();
            TempData["success"] = "Category edited successfully!";
            return RedirectToPage("Index");
        }
    }
}
