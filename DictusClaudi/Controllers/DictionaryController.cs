using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DictusClaudi.Data;
using DictusClaudi.Models;

namespace DictusClaudi.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DictionaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dictionary
        public async Task<IActionResult> Index()
        {
              return _context.DictEntry != null ? 
                          View(await _context.DictEntry.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.DictEntry'  is null.");
        }

        // GET: Dictionary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DictEntry == null)
            {
                return NotFound();
            }

            var dictEntry = await _context.DictEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dictEntry == null)
            {
                return NotFound();
            }

            return View(dictEntry);
        }

        // GET: Dictionary/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dictionary/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WordStem,WordTranslation")] DictEntry dictEntry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dictEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dictEntry);
        }

        // GET: Dictionary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DictEntry == null)
            {
                return NotFound();
            }

            var dictEntry = await _context.DictEntry.FindAsync(id);
            if (dictEntry == null)
            {
                return NotFound();
            }
            return View(dictEntry);
        }

        // POST: Dictionary/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WordStem,WordTranslation")] DictEntry dictEntry)
        {
            if (id != dictEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dictEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DictEntryExists(dictEntry.Id))
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
            return View(dictEntry);
        }

        // GET: Dictionary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DictEntry == null)
            {
                return NotFound();
            }

            var dictEntry = await _context.DictEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dictEntry == null)
            {
                return NotFound();
            }

            return View(dictEntry);
        }

        // POST: Dictionary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DictEntry == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DictEntry'  is null.");
            }
            var dictEntry = await _context.DictEntry.FindAsync(id);
            if (dictEntry != null)
            {
                _context.DictEntry.Remove(dictEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DictEntryExists(int id)
        {
          return (_context.DictEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
