using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        public readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _db.categories.ToListAsync();
            return View(objCategoryList);
        }
        //get
        public async Task<IActionResult> Create()
        {    
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Nmae");
            }
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            await _db.categories.AddAsync(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

		//get
		public async Task<IActionResult> Edit(int? id)
		{
            if(id== null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb= await _db.categories.FindAsync(id);
            //var categoryFromDbFirst=await _db.categories.FirstOrDefaultAsync(u=>u.Id==id);

            if(categoryFromDb == null)
                return NotFound();

			return View(categoryFromDb);
		}
		//post
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Nmae");
			}
			if (!ModelState.IsValid)
			{
				return View(obj);
			}
			_db.categories.Update(obj);
			await _db.SaveChangesAsync();
			TempData["success"] = "Category edited successfully";

			return RedirectToAction("Index");
		}


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _db.categories.FindAsync(id);
            //var categoryFromDbFirst=await _db.categories.FirstOrDefaultAsync(u=>u.Id==id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj=await _db.categories.FindAsync(id);
            if (obj==null)
            {
                return NotFound();
            }

            _db.categories.Remove(obj);
            await _db.SaveChangesAsync();
			TempData["success"] = "Category deleted successfully";

			return RedirectToAction("Index");
        }
    }
}
