using Microsoft.AspNetCore.Mvc;

namespace FactoryManager.Controllers
{
  public class HomeController : Controller
  {

    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

  }
}