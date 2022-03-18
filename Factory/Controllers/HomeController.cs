using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Factory.Controllers
{

  private readonly ToDoListContext _db;
  public class HomeController : Controller
  {

    public HomeController( FactoryContext db)
    {
      _db = db;
    }

    [HttpGet("/")]
    public ActionResult Index()
    {
      _db.Machines.ToList();
      _db.Engineers.ToList();
      return View();
    }

  }
}