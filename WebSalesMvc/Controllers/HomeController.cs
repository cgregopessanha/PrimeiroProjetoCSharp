using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Models;

namespace WebSalesMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Sales Web MVC APP";
            ViewData["Estudante"] = "Carlos Gregório Olivier Pessanha";
            ViewData["Email"] = "carlosgregoriopessanha@gmail.com";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Minha Página de Contato";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
