using Factory.Models;
using Microsoft.AspNetCore.Mvc;
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
  }
}