using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetClients")]
        public ActionResult<IEnumerable<Client>> GetClients()
        {
            var clients = _context.Clients.Include(s => s.Role).ToList();
            if (!clients.Any()) return BadRequest("Cписок пустой");
            return Ok(clients);
        }

        [HttpPost("AddClient")]
        public IActionResult AddClient(string firstName, string lastName, string username, string password, string email,
            string phone, long roleId)
        {
            Client client = new Client();
            client.FirstName = firstName;
            client.LastName = lastName;
            client.Username = username;
            client.Password = password;
            client.Email = email;
            client.Phone = phone;
            client.RoleId = roleId;

            var existingClient = _context.Clients.FirstOrDefault(s => s.FirstName == client.FirstName
                                                                && s.LastName == client.LastName
                                                                && s.Username == client.Username
                                                                && s.Password == client.Password
                                                                && s.Email == client.Email
                                                                && s.Phone == client.Phone);

            if (existingClient != null) return BadRequest("Такой клиент уже существует");

            _context.Clients.Add(client);
            _context.SaveChanges();

            return Ok("Клиент успешно добавлен");
        }

        [HttpDelete("DeleteClient")]
        public IActionResult DeleteClient(long id)
        {
            var client = _context.Clients.Find(id);

            if (client == null) return BadRequest($"Нет клиента с таким id: {id}");

            try
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Невозможно удалить клиента с id: {id}, так как она связана с другими объектами");
            }

            return Ok($"Клиент с id: {id} успешно удален");
        }

        [HttpPut("UpdateClient")]
        public IActionResult UpdateClient(long clientId, string? firstName = null, string? lastName = null,
            string? username = null, string? password = null, string? email = null,
            string? phone = null, long? roleId = null)
        {
            var findClient = _context.Clients.Find(clientId);

            if (findClient == null) return BadRequest("Клиент не найден");

            if (firstName != null)
            {
                findClient.FirstName = firstName;
            }
            if (lastName != null)
            {
                findClient.LastName = lastName;
            }
            if (username != null)
            {
                findClient.Username = username;
            }
            if (password != null)
            {
                findClient.Password = password;
            }
            if (email != null)
            {
                findClient.Email = email;
            }
            if (phone != null)
            {
                findClient.Phone = phone;
            }
            if (roleId != null)
            {
                findClient.RoleId = roleId.Value;
            }

            _context.Clients.Update(findClient);
            _context.SaveChanges();

            return Ok($"Клиент с id: {clientId} успешно обновлен");
        }
    }
}
