using HolaMundoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolaMundoMVC.Controllers
{
    public class AsignaturaController : Controller
    {
        private readonly EscuelaContext _context;
        public AsignaturaController(EscuelaContext context)
        {
            _context = context;
        }
        [Route("Asignatura/Index")]
        [Route("Asignatura/Index/{asignaturaId}")]
        public IActionResult Index(string asignaturaId)
        {
            if (!string.IsNullOrEmpty(asignaturaId))
            {
                var asignatura = from materia in _context.Asignaturas
                                 where materia.Id == asignaturaId
                                 select materia;
                return View(asignatura.SingleOrDefault());
            }
            else
            {
                return View("MultiAsignatura", _context.Asignaturas);
            }
        }
        public IActionResult MultiAsignatura()
        {
            ViewBag.Fecha = DateTime.Now;

            return View("MultiAsignatura", _context.Asignaturas);
        }
        public IActionResult Create()
        {
            var listaCursos = _context.Cursos.ToList();

            ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
            ViewBag.Fecha = DateTime.Now;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Asignatura asignatura)
        {
            ViewBag.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var asignaturaDb = new Asignatura()
                {
                    Nombre = asignatura.Nombre,
                    CursoId = asignatura.CursoId
                };
                _context.Asignaturas.Add(asignaturaDb);
                _context.SaveChanges();
                asignaturaDb.Curso = _context.Cursos.Find(asignatura.CursoId);
                ViewBag.MensajeExtra = "Asignatura Creada";
                return View("Index", asignaturaDb);
            }
            else
            {
                return View(asignatura);
            }
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var consulta = _context.Asignaturas.Find(id);
            if (!string.IsNullOrEmpty(id) && consulta != null)
            {
                var listaCursos = _context.Cursos.ToList();

                ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
                return View(consulta);
            }
            else
            {
                return RedirectToAction("MultiAsignatura");
            }
        }

        [HttpPost]
        public IActionResult Edit(Asignatura asignaturaViewModel)
        {
            var listaCursos = _context.Cursos.ToList();

            ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
            var asignaturaDB = _context.Asignaturas.Include(x => x.Curso).FirstOrDefault(x => x.Id == asignaturaViewModel.Id);
            asignaturaDB.Nombre = asignaturaViewModel.Nombre;
            asignaturaDB.CursoId = asignaturaViewModel.CursoId;
            _context.SaveChanges();
            return View("Index",asignaturaDB);
        }
    }
}
