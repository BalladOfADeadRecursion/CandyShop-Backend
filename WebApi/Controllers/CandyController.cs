using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CandyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandyController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetCandies")]
        public ActionResult<IEnumerable<Candy>> GetCandies()
        {
            var candies = _context.Candies.Include(s => s.Category).ToList();
            if (!candies.Any()) return BadRequest("Cписок пустой");
            return Ok(candies);
        }

        [HttpPost("AddCandy")]
        public IActionResult AddCandy(string name, int price, int size, int quantity, long categoryId)
        {
            Candy candy = new Candy();
            candy.Name = name;
            candy.Price = price;
            candy.Size = size;
            candy.Quantity = quantity;
            candy.CategoryId = categoryId;

            var categoryExists = _context.Categories.Any(c => c.Id == categoryId);
            if (!categoryExists) return BadRequest($"Нет категории с id: {categoryId}");

            var existingCandy = _context.Candies.FirstOrDefault(b => b.Name == name
                                                               && b.Price == price
                                                               && b.Size == size
                                                               && b.Quantity == quantity
                                                               && b.CategoryId == categoryId);

            if (existingCandy != null) return BadRequest("Такая сладость уже существует");

            _context.Candies.Add(candy);
            _context.SaveChanges();

            return Ok("Сладость успешно добавлена");
        }

        [HttpDelete("DeleteCandy")]
        public IActionResult DeleteCandy(long id)
        {
            var candy = _context.Candies.Find(id);

            if (candy == null)
            {
                return BadRequest("Нет сладости с таким id");
            }

            try
            {
                _context.Candies.Remove(candy);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Невозможно удалить сладость с id: {id}, так как она связана с другими объектами");
            }

            return Ok($"Категория c id: {id} успешно удалена");
        }

        [HttpPut("UpdateCandy")]
        public IActionResult UpdateCandy(long candyId, string? name = null, int? price = null,
            int? size = null, int? quantity = null, long? categoryId = null)
        {
            var findCandy = _context.Candies.Find(candyId);

            if (findCandy == null)
            {
                return NotFound("Сладость не найдена");
            }

            if (name != null) findCandy.Name = name;
            if (price != null) findCandy.Price = price.Value;
            if (size != null) findCandy.Size = size.Value;
            if (quantity != null) findCandy.Quantity = quantity.Value;
            if (categoryId != null)
            {
                var categoryExists = _context.Categories.Any(c => c.Id == categoryId);
                if (!categoryExists) return BadRequest($"Нет категории с id: {categoryId}");
                else findCandy.CategoryId = categoryId.Value;
            }

            _context.Candies.Update(findCandy);
            _context.SaveChanges();

            return Ok($"Сладость с id: {candyId} обновлена");
        }
    }
}
