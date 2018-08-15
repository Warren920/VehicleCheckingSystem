using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleTrackingSystem.DataModels;

namespace VehicleTrackingSystem.Controllers
{
    public class TireConditionsController : Controller
    {
        private readonly VehicleTrackingSystemContext _context;

        public TireConditionsController(VehicleTrackingSystemContext context)
        {
            _context = context;
        }

        // GET: TireConditions
        public async Task<IActionResult> Index()
        {
            return View(await _context.TireCondition.ToListAsync());
        }

        // GET: TireConditions/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tireCondition = await _context.TireCondition
                .FirstOrDefaultAsync(m => m.ConditionId == id);
            if (tireCondition == null)
            {
                return NotFound();
            }

            return View(tireCondition);
        }

        // GET: TireConditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TireConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConditionId,Condition")] TireCondition tireCondition)
        {
            if (IsTireConditionExistForAdd(tireCondition.Condition))
            {
                ModelState.AddModelError("Condition", "The tire condition is already exist");
            }
            if (ModelState.IsValid)
            {
                _context.Add(tireCondition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tireCondition);
        }

        // GET: TireConditions/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tireCondition = await _context.TireCondition.FindAsync(id);
            if (tireCondition == null)
            {
                return NotFound();
            }
            return View(tireCondition);
        }

        // POST: TireConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("ConditionId,Condition")] TireCondition tireCondition)
        {
            if (id != tireCondition.ConditionId)
            {
                return NotFound();
            }
            if (IsTireConditionForUpdate(tireCondition.Condition,tireCondition.ConditionId))
            {
                ModelState.AddModelError("Condition", "The tire condition is already exist.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tireCondition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TireConditionExists(tireCondition.ConditionId))
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
            return View(tireCondition);
        }

        // GET: TireConditions/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tireCondition = await _context.TireCondition
                .FirstOrDefaultAsync(m => m.ConditionId == id);
            if (tireCondition == null)
            {
                return NotFound();
            }

            return View(tireCondition);
        }

        // POST: TireConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var tireCondition = await _context.TireCondition.FindAsync(id);
            var isTireConditionIdInUse = _context.TireCondition.Where(t => t.ConditionId == id).Any();
            if (isTireConditionIdInUse)
            {
                ModelState.AddModelError("ConditionId", "The tire condition is already in use.");
                return View(tireCondition);
            }
            _context.TireCondition.Remove(tireCondition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TireConditionExists(byte id)
        {
            return _context.TireCondition.Any(e => e.ConditionId == id);
        }

        private bool IsTireConditionExistForAdd(string tireCondition)
        {
            return _context.TireCondition.Any(t => t.Condition == tireCondition);
        }

        private bool IsTireConditionForUpdate(string tireCondition,int conditionId)
        {
            return _context.TireCondition.Any(t => t.Condition == tireCondition && t.ConditionId != conditionId);
        }
    }
}
