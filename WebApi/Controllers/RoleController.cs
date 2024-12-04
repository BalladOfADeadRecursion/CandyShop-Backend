using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context) => _context = context;

        [HttpGet("GetRoles")]
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            var roles = _context.Roles.ToList();
            if (!roles.Any()) return BadRequest("Список пустой");
            return Ok(roles);
        }

        [HttpPost("AddRole")]
        public IActionResult AddRole(string name)
        {
            Role role = new Role();
            role.Name = name;

            var existingRole = _context.Roles.FirstOrDefault(b => b.Name == name);

            if (existingRole != null) return BadRequest("Такая роль уже существует");

            try
            {
                _context.Roles.Add(role);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest($"Невозможно удалить обувь с id: {role.Id}, так как она связана с другими объектами");
            }

            return Ok("Роль успешно добавлена");
        }

        [HttpDelete("DeleteRole")]
        public IActionResult DeleteRole(long id)
        {
            var role = _context.Roles.Find(id);

            if (role == null) return BadRequest("Нет роли с таким id");

            _context.Roles.Remove(role);
            _context.SaveChanges();

            return Ok($"Роль с id: {id} успешно удалена");
        }

        [HttpPut("UpdateRole")]
        public IActionResult UpdateRole(long roleId, string? name = null)
        {
            var findRole = _context.Roles.Find(roleId);

            if (findRole == null) return BadRequest($"Роль с id: {roleId} не найдена");

            if (name != null) findRole.Name = name;

            _context.Roles.Update(findRole);
            _context.SaveChanges();

            return Ok($"Роль с id: {roleId} успешно обновлена");
        }

    }
}
