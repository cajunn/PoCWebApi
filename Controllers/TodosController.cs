using PoCWebApi.Data;
using PoCWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace PoCWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // require a valid JWT
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TodosController(AppDbContext db) => _db = db;

        private string? GetOid() =>
            User.FindFirstValue("oid") ??
            User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");

        // GET api/todos/whoami
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var name = User.Identity?.Name;
            var oid = GetOid();
            return Ok(new { name, oid, claims = User.Claims.Select(c => new { c.Type, c.Value }) });
        }

        // GET api/todos
        [HttpGet]
        [Authorize(Policy = "perm:Todo.Read")]
        public async Task<IActionResult> GetMyTodos()
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();
            var list = await _db.Todos.Where(t => t.OwnerOid == oid).OrderBy(t => t.Id).ToListAsync();
            return Ok(list);
        }

        // POST api/todos
        [HttpPost]
        [Authorize(Policy = "perm:Todo.Write")]
        public async Task<IActionResult> Create([FromBody] CreateTodo dto)
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();

            var todo = new TodoItem { OwnerOid = oid, Title = dto.Title, IsDone = false };
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMyTodos), new { id = todo.Id }, todo);
        }

        // PATCH api/todos/{id}/done
        [HttpPatch("{id:int}/done")]
        [Authorize(Policy = "perm:Todo.Write")]
        public async Task<IActionResult> MarkDone(int id)
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();

            var todo = await _db.Todos.FirstOrDefaultAsync(t => t.Id == id && t.OwnerOid == oid);
            if (todo is null) return NotFound();
            todo.IsDone = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public record CreateTodo(string Title);
    }
}
