using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoCWebApi.Areas.Admin.ViewModels;
using PoCWebApi.Auth.Models;
using PoCWebApi.Data;

namespace PoCWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class UserRolesController : Controller
    {
        private readonly AppDbContext _db;

        public UserRolesController(AppDbContext db)
        {
            _db = db;
        }

        // Simple list of users to pick from
        // GET: /Admin/UserRoles
        public async Task<IActionResult> Index()
        {
            var users = await _db.Users
                .OrderBy(u => u.DisplayName)
                .ToListAsync();

            return View(users);
        }

        // GET: /Admin/UserRoles/Edit/{oid}
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Oid == id);

            if (user == null)
                return NotFound();

            var allRoles = await _db.Roles
                .OrderBy(r => r.Name)
                .ToListAsync();

            var vm = new UserRolesViewModel
            {
                Oid = user.Oid,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Roles = allRoles.Select(r => new RoleCheckboxItem
                {
                    RoleId = r.RoleId,
                    Name = r.Name,
                    IsAssigned = user.UserRoles.Any(ur => ur.RoleId == r.RoleId)
                }).ToList()
            };

            return View(vm);
        }

        // POST: /Admin/UserRoles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRolesViewModel model)
        {
            var user = await _db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Oid == model.Oid);

            if (user == null)
                return NotFound();

            // Clear existing
            _db.UserRoles.RemoveRange(user.UserRoles);

            var selectedRoleIds = model.Roles
                .Where(r => r.IsAssigned)
                .Select(r => r.RoleId)
                .ToList();

            foreach (var roleId in selectedRoleIds)
            {
                _db.UserRoles.Add(new UserRole
                {
                    Oid = user.Oid,
                    RoleId = roleId
                });
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
