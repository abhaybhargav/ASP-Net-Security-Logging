using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SecurityLoggingDemo.Data;
using SecurityLoggingDemo.Models;
using SecurityLoggingDemo.Services;

namespace SecurityLoggingDemo.Controllers
{
    [Authorize]
    public class HealthcareInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurityLogger _securityLogger;

        public HealthcareInfoController(ApplicationDbContext context, ISecurityLogger securityLogger)
        {
            _context = context;
            _securityLogger = securityLogger;
        }

        // GET: HealthcareInfo
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var healthcareInfos = await _context.HealthcareInfos.Where(h => h.UserId == userId).ToListAsync();
            await _securityLogger.LogHealthcareInfoAccessAsync(userId, "View All", 0);
            return View(healthcareInfos);
        }

        // GET: HealthcareInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var healthcareInfo = await _context.HealthcareInfos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (healthcareInfo == null)
            {
                return NotFound();
            }

            await _securityLogger.LogHealthcareInfoAccessAsync(userId, "View Details", healthcareInfo.Id);
            return View(healthcareInfo);
        }

        // GET: HealthcareInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HealthcareInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Condition,Description,DiagnosisDate")] HealthcareInfo healthcareInfo)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                healthcareInfo.UserId = userId;
                _context.Add(healthcareInfo);
                await _context.SaveChangesAsync();
                await _securityLogger.LogHealthcareInfoChangeAsync(userId, "Create", healthcareInfo.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(healthcareInfo);
        }

        // GET: HealthcareInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var healthcareInfo = await _context.HealthcareInfos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (healthcareInfo == null)
            {
                return NotFound();
            }

            await _securityLogger.LogHealthcareInfoAccessAsync(userId, "Edit Form", healthcareInfo.Id);
            return View(healthcareInfo);
        }

        // POST: HealthcareInfo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Condition,Description,DiagnosisDate")] HealthcareInfo healthcareInfo)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (id != healthcareInfo.Id || healthcareInfo.UserId != userId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    healthcareInfo.UserId = userId;
                    healthcareInfo.UpdatedAt = DateTime.UtcNow;
                    _context.Update(healthcareInfo);
                    await _context.SaveChangesAsync();
                    await _securityLogger.LogHealthcareInfoChangeAsync(userId, "Edit", healthcareInfo.Id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthcareInfoExists(healthcareInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(healthcareInfo);
        }

        // GET: HealthcareInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var healthcareInfo = await _context.HealthcareInfos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (healthcareInfo == null)
            {
                return NotFound();
            }

            await _securityLogger.LogHealthcareInfoAccessAsync(userId, "Delete Form", healthcareInfo.Id);
            return View(healthcareInfo);
        }

        // POST: HealthcareInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var healthcareInfo = await _context.HealthcareInfos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (healthcareInfo != null)
            {
                _context.HealthcareInfos.Remove(healthcareInfo);
                await _context.SaveChangesAsync();
                await _securityLogger.LogHealthcareInfoChangeAsync(userId, "Delete", id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool HealthcareInfoExists(int id)
        {
            return _context.HealthcareInfos.Any(e => e.Id == id);
        }
    }
}