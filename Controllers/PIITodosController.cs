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
    public class PIITodosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PIITodosController(AppDbContext db) => _db = db;

        private string? GetOid() =>
            User.FindFirstValue("oid") ??
            User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");

        // GET api/piitodos
        [HttpGet]
        public async Task<IActionResult> GetMyPIITodos()
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();
            var list = await _db.PIITodos.Where(t => t.OwnerOid == oid).OrderBy(t => t.Id).ToListAsync();
            return Ok(list);
        }

        // POST api/piitodos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePIITodo dto)
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();

            var piiTodo = new PIITodoItem { OwnerOid = oid, PIITitle = dto.Title, IsDone = false };
            _db.PIITodos.Add(piiTodo);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMyPIITodos), new { id = piiTodo.Id }, piiTodo);
        }

        // PATCH api/piitodos/{id}/done
        [HttpPatch("{id:int}/done")]
        public async Task<IActionResult> MarkDone(int id)
        {
            var oid = GetOid();
            if (string.IsNullOrEmpty(oid)) return Forbid();

            var piiTodo = await _db.PIITodos.FirstOrDefaultAsync(t => t.Id == id && t.OwnerOid == oid);
            if (piiTodo is null) return NotFound();
            piiTodo.IsDone = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public record CreatePIITodo(string Title);
    }
}
