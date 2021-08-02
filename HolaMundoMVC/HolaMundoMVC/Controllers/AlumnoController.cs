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
    public class AlumnoController : Controller
    {
        private readonly EscuelaContext _context;
        public AlumnoController(EscuelaContext context)
        {
            _context = context;
        }
        //[Route("Alumno/Index")]
        //[Route("Alumno/Index/{id}")]
        public IActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var alumnado = _context.Alumnos.Include(x => x.Curso).FirstOrDefault(x => x.Id == id);
                               
                return View(alumnado);
            }
            else
            {
                return View("MultiAlumno", _context.Alumnos);
            }
        }
        public IActionResult MultiAlumno()
        {
            var listaAlumno = _context.Alumnos.ToList<Alumno>();
            ViewBag.Fecha = DateTime.Now;

            return View("MultiAlumno", _context.Alumnos);
        }
        public IActionResult Create()
        {
            var listaCursos = _context.Cursos.ToList();

            ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
            ViewBag.Fecha = DateTime.Now;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Alumno alumno)
        {
            ViewBag.Fecha = DateTime.Now;
            if (ModelState.IsValid)
            {
                var alumnoDb = new Alumno() {
                    Nombre= alumno.Nombre,
                    CursoId = alumno.CursoId
                };
                _context.Alumnos.Add(alumnoDb);
                _context.SaveChanges();
                alumnoDb.Curso = _context.Cursos.Find(alumno.CursoId);
                ViewBag.MensajeExtra = "Alumno Agregado";
                return View("Index", alumnoDb);
            }
            else
            {
                return View(alumno);
            }
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
                if (!string.IsNullOrEmpty(id))
            {
                var listaCursos =_context.Cursos.ToList();

                ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
                var alumnado = _context.Alumnos.Include(x => x.Curso).SingleOrDefault(x => x.Id == id);
                
                return View("Edit",alumnado);
            }
            else
            {
                return RedirectToAction("MultiAlumno");
            }
        }

        [HttpPost]
        public IActionResult Edit(Alumno alumnoViewModel)
        {
            if (ModelState.IsValid)
            {
                var alumnodb = _context.Alumnos.Include(x => x.Curso).FirstOrDefault(x=>x.Id== alumnoViewModel.Id);
                alumnodb.Nombre = alumnoViewModel.Nombre;
                alumnodb.Curso = _context.Cursos.Find(alumnoViewModel.CursoId);
                alumnodb.CursoId = alumnoViewModel.CursoId;
                _context.SaveChanges();
                return View("Index", alumnodb);
            }
            else
            {
                var listaCursos = from cursos in _context.Cursos
                                  select cursos;
                ViewBag.cursos = new SelectList(listaCursos, "Id", "Nombre");
                return View(alumnoViewModel);
            }
           
        }
    }
}
