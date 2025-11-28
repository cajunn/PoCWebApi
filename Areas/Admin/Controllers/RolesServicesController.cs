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
    public class RoleServicesController : Controller
    {
        private readonly AppDbContext _db;

        public RoleServicesController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/RoleServices/Edit/2
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _db.Roles
                .Include(r => r.RoleServices)
                .FirstOrDefaultAsync(r => r.RoleId == id);

            if (role == null)
                return NotFound();

            var allServices = await _db.Services
                .OrderBy(s => s.Key)
                .ToListAsync();

            var vm = new RoleServicesViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.Name,
                Services = allServices.Select(s => new ServiceCheckboxItem
                {
                    ServiceId = s.ServiceId,
                    Key = s.Key,
                    Description = s.Description,
                    IsAssigned = role.RoleServices.Any(rs => rs.ServiceId == s.ServiceId)
                }).ToList()
            };

            return View(vm);
        }

        // POST: /Admin/RoleServices/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleServicesViewModel model)
        {
            var role = await _db.Roles
                .Include(r => r.RoleServices)
                .FirstOrDefaultAsync(r => r.RoleId == model.RoleId);

            if (role == null)
                return NotFound();

            // Clear existing mappings
            _db.RoleServices.RemoveRange(role.RoleServices);

            // Add selected ones
            var selectedServiceIds = model.Services
                .Where(s => s.IsAssigned)
                .Select(s => s.ServiceId)
                .ToList();

            foreach (var serviceId in selectedServiceIds)
            {
                _db.RoleServices.Add(new RoleService
                {
                    RoleId = role.RoleId,
                    ServiceId = serviceId
                });
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Roles", new { area = "Admin" });
        }
    }
}
