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
    public class VehiclesController : Controller
    {
      
        private readonly VehicleTrackingSystemContext _context;

        public VehiclesController(VehicleTrackingSystemContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            
            var vehicles = _context.Vehicle.Include(v => v.TireCondition).Include(v => v.VehicleType);
            //Condition cannot display, create IdNumber throw exception not handle

            return View(await vehicles.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.TireCondition)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Vid == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            ViewData["ConditionId"] = new SelectList(_context.TireCondition, "ConditionId", "Condition");
            ViewData["TypeId"] = new SelectList(_context.VehicleType, "TypeId", "TypeName");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Vid,TypeId,FuelLevel,Idnumber,ConditionId,TireQty")] Vehicle vehicle)
        {
            if (IsVehicleExistforAdd(vehicle.Idnumber))
            {
                ModelState.AddModelError("Idnumber", "VIN/HIN already exists.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ConditionId"] = new SelectList(_context.TireCondition, "ConditionId", "Condition", vehicle.ConditionId);
            ViewData["TypeId"] = new SelectList(_context.VehicleType, "TypeId", "TypeName", vehicle.TypeId);
            return View(vehicle);
        }
        
        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            ViewData["ConditionId"] = new SelectList(_context.TireCondition, "ConditionId", "Condition", vehicle.ConditionId);
            ViewData["TypeId"] = new SelectList(_context.VehicleType, "TypeId", "TypeName", vehicle.TypeId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Vid,TypeId,FuelLevel,Idnumber,ConditionId,TireQty")] Vehicle vehicle)
        {
            if (id != vehicle.Vid)
            {
                return NotFound();
            }
            if (IsVehicleExistforUpdate(vehicle.Idnumber,vehicle.Vid)){
                ModelState.AddModelError("Idnumber","The VIN/HIN is already exists.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Vid))
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
            ViewData["ConditionId"] = new SelectList(_context.TireCondition, "ConditionId", "Condition", vehicle.ConditionId);
            ViewData["TypeId"] = new SelectList(_context.VehicleType, "TypeId", "TypeName", vehicle.TypeId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.TireCondition)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Vid == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Vid == id);
        }

        private bool IsVehicleExistforAdd(string vin)
        {
            return _context.Vehicle.Where(v => v.Idnumber == vin).Any();
        }
        private bool IsVehicleExistforUpdate(string vin, int vID)
        {
            return _context.Vehicle.Where(v => v.Idnumber == vin && v.Vid != vID).Any();
        }
    }
}
