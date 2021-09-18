using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MAV.Web.Data;
using MAV.Web.Data.Entities;

namespace MAV.Web.Controllers
{
    public class InternsController : Controller
    {
        private readonly DataContext _context;

        public InternsController(DataContext context)
        {
            _context = context;
        }

        // GET: Interns
        public async Task<IActionResult> Index()
        {
            return View(await _context.Interns.ToListAsync());
        }

        // GET: Interns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Interns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (intern == null)
            {
                return NotFound();
            }

            return View(intern);
        }

        // GET: Interns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Interns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Intern intern)
        {
            if (ModelState.IsValid)
            {
                _context.Add(intern);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(intern);
        }

        // GET: Interns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Interns.FindAsync(id);
            if (intern == null)
            {
                return NotFound();
            }
            return View(intern);
        }

        // POST: Interns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Intern intern)
        {
            if (id != intern.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(intern);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternExists(intern.Id))
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
            return View(intern);
        }

        // GET: Interns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intern = await _context.Interns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (intern == null)
            {
                return NotFound();
            }

            return View(intern);
        }

        // POST: Interns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intern = await _context.Interns.FindAsync(id);
            _context.Interns.Remove(intern);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternExists(int id)
        {
            return _context.Interns.Any(e => e.Id == id);
        }
    }
}
