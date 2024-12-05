// Подключение необходимых пространств имен
using Microsoft.AspNetCore.Mvc; // Для использования MVC контроллеров и атрибутов
using Microsoft.EntityFrameworkCore; // Для работы с Entity Framework
using WebApi.Models; // Для использования моделей данных

// Определение пространства имен для контроллера API
namespace WebApi.Controllers
{
    // Атрибут, указывающий на использование контроллера API
    [ApiController]
    // Маршрут для этого контроллера, будет доступен по /api/Candy
    [Route("/api/[controller]")]
    public class CandyController : Controller
    {
        // Переменная для доступа к контексту базы данных
        private readonly ApplicationDbContext _context;

        // Конструктор для инициализации контекста базы данных
        public CandyController(ApplicationDbContext context) => _context = context;

        // Метод для получения списка всех сладостей
        [HttpGet("GetCandies")]
        public ActionResult<IEnumerable<Candy>> GetCandies()
        {
            // Получение списка всех сладостей с включением категории каждой сладости
            var candies = _context.Candies.Include(s => s.Category).ToList();

            // Проверка, есть ли сладости в базе данных
            if (!candies.Any()) return BadRequest("Cписок пустой");

            // Возвращение списка сладостей с кодом 200 (OK)
            return Ok(candies);
        }

        // Метод для добавления новой сладости
        [HttpPost("AddCandy")]
        public IActionResult AddCandy(string name, int price, int size, int quantity, long categoryId)
        {
            // Создание нового объекта Candy
            Candy candy = new Candy();
            candy.Name = name; // Установка имени сладости
            candy.Price = price; // Установка цены сладости
            candy.Size = size; // Установка размера сладости
            candy.Quantity = quantity; // Установка количества сладости
            candy.CategoryId = categoryId; // Установка категории для сладости

            // Проверка, существует ли категория с заданным id
            var categoryExists = _context.Categories.Any(c => c.Id == categoryId);
            if (!categoryExists) return BadRequest($"Нет категории с id: {categoryId}");

            // Проверка, существует ли уже такая сладость с одинаковыми параметрами
            var existingCandy = _context.Candies.FirstOrDefault(b => b.Name == name
                                                               && b.Price == price
                                                               && b.Size == size
                                                               && b.Quantity == quantity
                                                               && b.CategoryId == categoryId);

            // Если такая сладость уже есть, возвращаем ошибку
            if (existingCandy != null) return BadRequest("Такая сладость уже существует");

            // Добавляем новую сладость в базу данных
            _context.Candies.Add(candy);
            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            // Возвращаем сообщение об успешном добавлении
            return Ok("Сладость успешно добавлена");
        }

        // Метод для удаления сладости по id
        [HttpDelete("DeleteCandy")]
        public IActionResult DeleteCandy(long id)
        {
            // Поиск сладости в базе данных по id
            var candy = _context.Candies.Find(id);

            // Если сладость не найдена, возвращаем ошибку
            if (candy == null)
            {
                return BadRequest("Нет сладости с таким id");
            }

            // Попытка удаления сладости
            try
            {
                _context.Candies.Remove(candy); // Удаляем сладость
                _context.SaveChanges(); // Сохраняем изменения
            }
            catch (DbUpdateException ex) // В случае ошибки при сохранении (например, если сладость связана с другими объектами)
            {
                return BadRequest($"Невозможно удалить сладость с id: {id}, так как она связана с другими объектами");
            }

            // Возвращаем сообщение об успешном удалении
            return Ok($"Категория c id: {id} успешно удалена");
        }

        // Метод для обновления данных сладости
        [HttpPut("UpdateCandy")]
        public IActionResult UpdateCandy(long candyId, string? name = null, int? price = null,
            int? size = null, int? quantity = null, long? categoryId = null)
        {
            // Поиск сладости по id
            var findCandy = _context.Candies.Find(candyId);

            // Если сладость не найдена, возвращаем ошибку
            if (findCandy == null)
            {
                return NotFound("Сладость не найдена");
            }

            // Обновляем параметры сладости, если они не равны null
            if (name != null) findCandy.Name = name;
            if (price != null) findCandy.Price = price.Value;
            if (size != null) findCandy.Size = size.Value;
            if (quantity != null) findCandy.Quantity = quantity.Value;

            // Проверка категории, если она была передана
            if (categoryId != null)
            {
                // Проверка, существует ли категория с таким id
                var categoryExists = _context.Categories.Any(c => c.Id == categoryId);
                if (!categoryExists) return BadRequest($"Нет категории с id: {categoryId}");
                else findCandy.CategoryId = categoryId.Value; // Обновляем категорию
            }

            // Обновление сладости в базе данных
            _context.Candies.Update(findCandy);
            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            // Возвращаем сообщение об успешном обновлении
            return Ok($"Сладость с id: {candyId} обновлена");
        }
    }
}
