using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Models;
using PasswordManager.Helpers;
using Microsoft.AspNetCore.Identity;


namespace PasswordManager.Controllers
{
    public class PasswordEntriesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PasswordEntriesController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: PasswordEntries
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var entries = await _context.PasswordEntries
            .Where(e => e.UserId == userId)
            .ToListAsync();

            return View(entries);
        }

        // GET: PasswordEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passwordEntry = await _context.PasswordEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passwordEntry == null)
            {
                return NotFound();
            }

            return View(passwordEntry);
        }

        // GET: PasswordEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PasswordEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceName,Username,EncryptedPassword,Notes,CreatedAt")] PasswordEntry passwordEntry)
        {
            if (ModelState.IsValid)
            {
                passwordEntry.UserId = _userManager.GetUserId(User);
                _context.Add(passwordEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(passwordEntry);
        }
        // GET: PasswordEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passwordEntry = await _context.PasswordEntries.FindAsync(id);
            if (passwordEntry == null)
            {
                return NotFound();
            }
            return View(passwordEntry);
        }

        // POST: PasswordEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceName,Username,EncryptedPassword,Notes,CreatedAt,UserId")] PasswordEntry passwordEntry)
        {
            if (id != passwordEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    passwordEntry.EncryptedPassword = EncryptionHelper.Encrypt(passwordEntry.EncryptedPassword);
                    _context.Update(passwordEntry);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasswordEntryExists(passwordEntry.Id))
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
            return View(passwordEntry);
        }

        // GET: PasswordEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passwordEntry = await _context.PasswordEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passwordEntry == null)
            {
                return NotFound();
            }

            return View(passwordEntry);
        }

        // POST: PasswordEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passwordEntry = await _context.PasswordEntries.FindAsync(id);
            if (passwordEntry != null)
            {
                _context.PasswordEntries.Remove(passwordEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasswordEntryExists(int id)
        {
            return _context.PasswordEntries.Any(e => e.Id == id);
        }
    }
}
