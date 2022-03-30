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

    public ActionResult Edit(int id)
    {
      Engineer thisEng = _db.Engineers.FirstOrDefault(m => m.EngineerId == id);

      return View(thisEng);
    }

    [HttpPost]
    public ActionResult Edit(Engineer eng)
    {
      if (eng != null)
      {
       _db.Entry(eng).State = EntityState.Modified;
       _db.SaveChanges();
      }

      return RedirectToAction("Details", new {id = eng.EngineerId});
    }

    public ActionResult Delete(int id)
    {
      Engineer thisEng = _db.Engineers.FirstOrDefault(m => m.EngineerId == id);

      return View(thisEng);
    }

    [HttpPost]
    public ActionResult Delete(Engineer eng)
    {
      if (eng != null)
      {
        _db.Engineers.Remove(eng);
        _db.SaveChanges();
      }

      return RedirectToAction("Index");
    }


    public ActionResult AddMachine(int id)
    {
      Engineer thisEng = _db.Engineers.FirstOrDefault(eng => eng.EngineerId == id);
      ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
      return View(thisEng);
    }

    [HttpPost]
    public ActionResult AddMachine(Engineer eng, int MachineId)
    {
      if (MachineId != 0)
      {
        if (_db.EngineerMachines.Where(engM => engM.EngineerId == eng.EngineerId && engM.MachineId == MachineId).ToList().Count() == 0)
        {
          _db.EngineerMachines.Add( new EngineerMachine(){EngineerId = eng.EngineerId, MachineId = MachineId} );
          _db.SaveChanges();
        }
      }

      return RedirectToAction("Details", new {id = eng.EngineerId});
    }

    public ActionResult RemoveMachine(int id)
    {
      Engineer thisEng = _db.Engineers.FirstOrDefault(m => m.EngineerId == id);

      var assignedMachines = _db.EngineerMachines
                              .Where(e => e.EngineerId == id)
                              .Select(m => m.Machine).ToList();
      ViewBag.MachineId = new SelectList(assignedMachines, "MachineId", "Name");
      return View(thisEng);
    }

    [HttpPost]
    public ActionResult RemoveMachine(Engineer eng, int MachineId)
    {
      if(MachineId != 0)
      {
        EngineerMachine thisEngM = _db.EngineerMachines.FirstOrDefault(m => m.MachineId == MachineId && m.EngineerId == eng.EngineerId);

        if (thisEngM != null)
        {
          _db.EngineerMachines.Remove(thisEngM);
          _db.SaveChanges();
        }
      }
      return RedirectToAction("Details", new {id = eng.EngineerId});
    }
  }
}