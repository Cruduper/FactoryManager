using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
// using System;

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
        _db.EngineerMachines.Add(new EngineerMachine(){EngineerId = EngineerId, MachineId = mach.MachineId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }


  }
}