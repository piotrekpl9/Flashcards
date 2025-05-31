using Microsoft.AspNetCore.Mvc;

namespace Flashcards.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}