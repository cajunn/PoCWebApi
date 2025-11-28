using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoCWebApi.Auth.Models;
using PoCWebApi.Data;

namespace PoCWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class RolesController : Controller
    {
        private readonly AppDbContext _db;

        public RolesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _db.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();

            return View(roles);
        }

        // GET: /Admin/Roles/Create
        public IActionResult Create()
        {
            return View(new Role());
        }

        // POST: /Admin/Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _db.Roles.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}