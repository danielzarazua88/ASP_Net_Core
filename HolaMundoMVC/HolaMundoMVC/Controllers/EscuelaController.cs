using HolaMundoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolaMundoMVC.Controllers
{
    public class EscuelaController : Controller
    {
            private EscuelaContext _context;
        public IActionResult Index()
        {
            ViewBag.CosaDinamica = "Super Escuela";
            var escuela = _context.Escuelas.FirstOrDefault();
            return View(escuela);
        }
        public EscuelaController(EscuelaContext context)
        {
            _context = context;
        }
    }
}
