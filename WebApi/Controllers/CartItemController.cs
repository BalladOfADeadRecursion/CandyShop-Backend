using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CartItemController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartItemController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetCartItems")]
        public ActionResult<IEnumerable<CartItem>> GetCartItems()
        {
            var cartItems = _context.CartItems.Include(s => s.Cart).Include(s => s.Candy).ToList();
            if (!cartItems.Any()) return BadRequest("Cписок пустой");
            return Ok(cartItems);
        }

        [HttpPost("AddCartItem")]
        public IActionResult AddCartItem(long cartId, long candyId, int quantity)
        {
            CartItem cartItem = new CartItem();
            cartItem.CartId = cartId;
            cartItem.CandyId = candyId;
            cartItem.Quantity = quantity;

            var candy = _context.Candies.Find(candyId);

            if (candy.Quantity < quantity) return BadRequest("Сладостей не достаточно");

            candy.Quantity -= quantity;
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return Ok("Сладость успешно добавлена в корзину");
        }

        [HttpDelete("DeleteCartItem")]
        public IActionResult DeleteCartItem(long id)
        {
            var cartItem = _context.CartItems.Include(ci => ci.Candy).FirstOrDefault(ci => ci.Id == id);

            if (cartItem == null) return BadRequest("Нет записи в корзине с таким id");

            var shoe = cartItem.Candy;
            shoe.Quantity += cartItem.Quantity;

            try
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Невозможно удалить объект корзины с id: {id}, так как она связана с другими объектами");
            }

            return Ok($"Элемент корзины успешно удален. Возвращено {cartItem.Quantity} сладостей.");
        }

        [HttpPut("UpdateCartItem")]
        public IActionResult UpdateCartItem(long cartItemId, long? cartId = null, long? candyId = null, int? quantity = null)
        {
            var findCartItem = _context.CartItems.Include(ci => ci.Candy).FirstOrDefault(ci => ci.Id == cartItemId);

            if (findCartItem == null) return BadRequest("Запись в корзине не найдена");

            if (quantity != null)
            {
                var shoe = findCartItem.Candy;
                int oldQuantity = findCartItem.Quantity;
                int newQuantity = quantity.Value;
                int quantityDifference = newQuantity - oldQuantity;

                if (quantityDifference > 0 && shoe.Quantity < quantityDifference)
                {
                    return BadRequest("Обуви недостаточно на складе для увеличения количества");
                }

                shoe.Quantity -= quantityDifference;
                findCartItem.Quantity = newQuantity;
            }

            if (candyId != null && findCartItem.CandyId != candyId.Value)
            {
                var oldShoe = findCartItem.Candy;
                var newShoe = _context.Candies.Find(candyId.Value);

                if (newShoe == null) return BadRequest("Новая обувь не найдена");

                oldShoe.Quantity += findCartItem.Quantity;

                if (newShoe.Quantity < findCartItem.Quantity)
                {
                    return BadRequest("Новой обуви недостаточно на складе");
                }

                newShoe.Quantity -= findCartItem.Quantity;
                findCartItem.CandyId = candyId.Value;
                findCartItem.Candy = newShoe;
            }

            if (cartId != null) findCartItem.CartId = cartId.Value;

            _context.CartItems.Update(findCartItem);
            _context.SaveChanges();

            return Ok($"Объект корзины с id: {cartItemId} успешно обновлен");
        }
    }
}
