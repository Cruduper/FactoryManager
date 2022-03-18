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
      Machine model = _db.Machines.FirstOrDefault(m => m.MachineId == id);
      return View(model);
    }

    public ActionResult Create()
    {
      ViewBag.EngineerId = new selectList(_db.Engineers, "EngineerId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Machine mach)
    {
      _db.Machines.Add(mach);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    
  }
}