using HolaMundoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolaMundoMVC.Controllers
{
    public class CursoController : Controller
    {
        private readonly EscuelaContext _context;
        public CursoController(EscuelaContext context)
        {
            _context = context;
        }
        [Route("Curso/Index")]
        [Route("Curso/Index/{id}")]
        public IActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var curso = from cursito in _context.Cursos
                                 where cursito.Id == id
                                 select cursito;
                return View(curso.SingleOrDefault());
            }
            else
            {
                return View("MultiCurso", _context.Cursos);
            }
        }
        public IActionResult MultiCurso()
        {
            var listaCurso = _context.Cursos.ToList<Curso>();
            ViewBag.Fecha = DateTime.Now;

            return View("MultiCurso", _context.Cursos);
        }
        public IActionResult Create()
        {
            var listaCursos = _context.Cursos.ToList();

            ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
            ViewBag.Fecha = DateTime.Now;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Curso curso)
        {
            ViewBag.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
            var escuela = _context.Escuelas.FirstOrDefault();
            curso.EscuelaId = escuela.Id;
            _context.Cursos.Add(curso);
            _context.SaveChanges();
                ViewBag.MensajeExtra = "Curso Creado";
             return View("Index", curso);
            }
            else
            {
            return View(curso);
            }
        }
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Curso cursoViewModel)
        {
            var listaCursos = _context.Cursos.ToList();

            ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
            var cursoDB = _context.Asignaturas.Find(cursoViewModel.Id);
            cursoDB.Nombre = cursoViewModel.Nombre;
            _context.SaveChanges();
            return View("Index", cursoViewModel);
        }
    }
}
