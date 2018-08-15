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
    public class VehicleTypesController : Controller
    {
       private readonly VehicleTrackingSystemContext _context;

        public VehicleTypesController(VehicleTrackingSystemContext context)
        {
            _context = context;
        }

        // GET: VehicleTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleType.ToListAsync());
        }

        // GET: VehicleTypes/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName")] VehicleType vehicleType)
        {
            if(IsTypeNameExistForAdd(vehicleType.TypeName)){
                ModelState.AddModelError("TypeName", "The vehicle type is already exist.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(vehicleType);
                await _context.SaveChangesAsync();
                  return RedirectToAction(nameof(Index));
              //  return RedirectToAction("Create", "Vehicles");
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("TypeId,TypeName")] VehicleType vehicleType)
        {
            if (id != vehicleType.TypeId)
            {
                return NotFound();
            }
            if (IsTypeNameForUpdate(vehicleType.TypeName,vehicleType.TypeId))
            {
                ModelState.AddModelError("TypeName", "The vehicle type name is already exist.");
            }
      
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.TypeId))
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
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleType
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var vehicleType = await _context.VehicleType.FindAsync(id);
            var isVehicleTypeInUse= _context.Vehicle.Where(v => v.TypeId == id).Any();
            if (isVehicleTypeInUse)
            {
                ModelState.AddModelError("TypeId", "The vehicle type is already in use.");
                return View(vehicleType);
            }
            
            _context.VehicleType.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(byte id)
        {
            return _context.VehicleType.Any(e => e.TypeId == id);
        }
        private bool IsTypeNameExistForAdd(string typeName)
        {
            return _context.VehicleType.Any(v => v.TypeName == typeName);
        }
        private bool IsTypeNameForUpdate(string typeName, int typeId)
        {
            return _context.VehicleType.Where(v => v.TypeName == typeName && v.TypeId!=typeId).Any();
        }


    }
}
