using Factory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Factory.Controllers
{
  public class HomeController : Controller
  {
    private readonly FactoryContext _db;

    public HomeController( FactoryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      ViewBag.Machines = _db.Machines.ToList();
      ViewBag.Engineers = _db.Engineers.ToList();
      Console.WriteLine("Number of Machines: " + _db.Machines.ToList().Count() );
      return View(_db.Machines.ToList());
    }
  }
}