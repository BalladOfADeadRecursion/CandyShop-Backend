using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetCategories")]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _context.Categories.ToList();
            if (!categories.Any()) return BadRequest("Cписок пустой");
            return Ok(categories);
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(string name)
        {
            Category category = new Category();
            category.Name = name;

            var existingCategory = _context.Categories.FirstOrDefault(b => b.Name == name);

            if (existingCategory != null) return BadRequest("Такой бренд уже существует");

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Категория успешно добавлена");
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(long id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return BadRequest("Нет категории с таким id");
            }

            try
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Невозможно удалить категория с id: {id}, так как она связана с другими объектами");
            }

            return Ok($"Категория c id: {id} успешно удалена");
        }

        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory(long categoryId, string? name = null)
        {
            var findCategory = _context.Categories.Find(categoryId);

            if (findCategory == null)
            {
                return NotFound("Категория не найден");
            }

            if (name != null)
            {
                findCategory.Name = name;
            }

            _context.Categories.Update(findCategory);
            _context.SaveChanges();

            return Ok($"Категория с id: {categoryId} обновлена");
        }
    }
}
