using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
// using System;

namespace Factory.Controllers
{
  public class EngineersController : Controller
  {
    private readonly FactoryContext _db;

    public EngineersController (FactoryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Engineer> model = _db.Engineers.ToList();
      return View(model);
    }

    public ActionResult Details(int id)
    {
      Engineer model = _db.Engineers
                        .Include(eng => eng.JoinEntities)
                        .ThenInclude(join => join.Machine)
                        .FirstOrDefault(m => m.EngineerId == id);
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Engineer eng, int MachineId)
    {
      _db.Engineers.Add(eng);
      _db.SaveChanges();

      if (MachineId != 0)
      {
        _db.EngineerMachines.Add(new EngineerMachine() { EngineerId = eng.EngineerId, MachineId = MachineId });
        _db.SaveChanges();
      }

      return RedirectToAction("Index");
    }

  }
}