using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IDS_School.Data;
using IDS_School.Models;
using IDS_School.Service;

namespace IDS_School.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        //  [Authorize(Roles = "Admin")]
        // GET: /Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(u => u.Department).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: /Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name"); ;
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            ViewData["DepartmentIds"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            return View(user);
        }

        // GET: /Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public async Task<IActionResult> AssignRole(string id)
        {
            if (id == null) return NotFound();
            var user = await _context.Users.Include(u => u.Department).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();
            var currentRoleIds = await _context.UserRoles.Where(u => u.UserId == user.Id).Select(u => u.RoleId).ToListAsync();
            var remainingRoleIds = await _context.Roles.Where(u => !currentRoleIds.Contains(u.Id)).Select(u => u.Id).ToListAsync();

            ViewData["RemainingRoleIds"] = remainingRoleIds;
            ViewData["CurrentRoleIds"] = currentRoleIds;

            return View(user);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> AddRole(string roleId, string userId)
        {
            //var existRole = (from x in _context.UserRoles
            //                 where x.UserId == userId || x.RoleId == roleId
            //                  select x).FirstOrDefault();
            //var role = existRole.RoleId;
            //var userid = existRole.UserId;


                var userrole = new IdentityUserRole<string>()
                {
                    UserId = userId,
                    RoleId = roleId
                };
                _context.Add(userrole);
                await _context.SaveChangesAsync();

            //else if (existRole != null && existRole.RoleId != roleId)
            //{

            //    existRole.RoleId = roleId;
            //    _context.Update(existRole);
            //    await _context.SaveChangesAsync();
            //}

            return RedirectToAction(nameof(AssignRole), new { id = userId });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> RemoveRole(string roleId, string userId)
        {
            var userrole = await _context.UserRoles.FirstOrDefaultAsync(u => u.UserId == userId && u.RoleId == roleId);
            if (userrole != null)
            {
                _context.Remove(userrole);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AssignRole), new { id = userId });
        }
    }
}