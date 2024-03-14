using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _db;
        public Category Category { get; set; }
        public DeleteModel(AppDbContext appDbContext)
        {
            _db = appDbContext;
        }
        public void OnGet(int? id)
        {
            if (id != null || id != 0)
            {
                Category = _db.Categories.Find(id);
            }

        }
        public IActionResult OnPost()
        {
            Category catFromDb = _db.Categories.Find(Category.Id);
            if (catFromDb == null)
            {
                return Page();
            }
            _db.Categories.Remove(catFromDb);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToPage("Index");
        }
    }
}
