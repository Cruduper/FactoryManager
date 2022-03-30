using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Factory.Controllers
{
  public class MachinesController : Controller
  {
    private readonly FactoryContext _db;

    public MachinesController (FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Machine> model = _db.Machines.ToList();
      return View(model);
    }

    public ActionResult Details(int id)
    {
      Machine model = _db.Machines
                        .Include(mach => mach.JoinEntities)
                        .ThenInclude(join => join.Engineer)
                        .FirstOrDefault(m => m.MachineId == id);
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine mach, int EngineerId)
    {
      _db.Machines.Add(mach);
      _db.SaveChanges();

      if (EngineerId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = mach.MachineId });
        _db.SaveChanges();
      }

      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      Machine thisMach = _db.Machines.FirstOrDefault(m => m.MachineId == id);

      return View(thisMach);
    }

    [HttpPost]
    public ActionResult Edit(Engineer mach)
    {
      if (mach != null)
      {
       _db.Entry(mach).State = EntityState.Modified;
       _db.SaveChanges();
      }

      return RedirectToAction("Details", new {id = mach.MachineId});
    }

    public ActionResult AddEngineer(int id)
    {
      Machine thisMach = _db.Machines.FirstOrDefault(mach => mach.MachineId == id);
      ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
      return View(thisMach);
    }

    [HttpPost]
    public ActionResult AddEngineer(Machine mach, int EngineerId)
    {
      if (EngineerId != 0)
      {
        if (_db.EngineerMachines.Where( engM => engM.EngineerId == EngineerId && engM.MachineId == mach.MachineId).ToList().Count() == 0 )
        {
          _db.EngineerMachines.Add(new EngineerMachine(){EngineerId = EngineerId, MachineId = mach.MachineId});
          _db.SaveChanges();
        }
      }
      return RedirectToAction("Details", new {id = mach.MachineId});
    }

    public ActionResult RemoveEngineer(int id)
    {
      Machine thisMach = _db.Machines.FirstOrDefault(m => m.MachineId == id);

      var assignedEngineers = _db.EngineerMachines
                                .Where(j => j.MachineId == id)
                                .Select(e => e.Engineer)
                                .ToList();

      ViewBag.EngineerId = new SelectList(assignedEngineers, "EngineerId", "Name");
      return View(thisMach); 
    }

    [HttpPost]
    public ActionResult RemoveEngineer(Machine mach, int EngineerId)
    {
      if (EngineerId != 0)
      {
        EngineerMachine thisEngM = _db.EngineerMachines.FirstOrDefault(m => m.MachineId == mach.MachineId && m.EngineerId == EngineerId);
        

        if (thisEngM != null)
        {
          _db.EngineerMachines.Remove(thisEngM);
          _db.SaveChanges();
        }
      }
      
      return RedirectToAction("Details", new {id = mach.MachineId});
    }


  }
}