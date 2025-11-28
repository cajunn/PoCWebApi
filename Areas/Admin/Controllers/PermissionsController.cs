using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoCWebApi.Auth.Models;
using PoCWebApi.Data;

namespace PoCWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class PermissionsController : Controller
    {
        private readonly AppDbContext _db;

        public PermissionsController(AppDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Permissions
        public async Task<IActionResult> Index()
        {
            var perms = await _db.Permissions
                .OrderBy(p => p.Key)
                .ToListAsync();

            return View(perms);
        }
    }
}