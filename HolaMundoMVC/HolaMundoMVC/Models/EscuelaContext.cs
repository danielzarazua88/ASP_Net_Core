using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HolaMundoMVC.Models
{
    public class EscuelaContext: DbContext
    {
        public DbSet<Escuela> Escuelas { get; set; }
        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }

        public EscuelaContext (DbContextOptions<EscuelaContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var escuela = new Escuela();
            escuela.AñoDeCreación = 2005;
            escuela.EscuelaId = Guid.NewGuid().ToString();
            escuela.Nombre = "Platzi Master";
            escuela.Dirección = "Platanito";
            escuela.Ciudad = "Bogota";
            escuela.Pais = "Colombia";
            escuela.TipoEscuela = TiposEscuela.Secundaria;

            modelBuilder.Entity<Escuela>().HasData(escuela);

            modelBuilder.Entity<Asignatura>().HasData(
                new Asignatura { Nombre = "Matematicas"},
                new Asignatura { Nombre = "Educacion Fisica" },
                new Asignatura { Nombre = "Español" },
                new Asignatura { Nombre = "Programacion" }
            );
            var cursos = GenerarCurso();
            modelBuilder.Entity<Curso>().HasData(cursos);
            Alumno[] alumnosInicio = GenerarAlumnosAlAzar(50, cursos.FirstOrDefault().Id).ToArray();
            modelBuilder.Entity<Alumno>().HasData(alumnosInicio);
        }
        private List<Curso> GenerarCurso()
        {
            string[] cursos = { "101", "201", "301", "401" };
            var curso = from c in cursos
                        select new Curso
                        {
                            Nombre = c
                        };
            return curso.ToList();
        }
        private List<Alumno> GenerarAlumnosAlAzar(
            int cantidad, string cursoid)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno
                               {
                                   Nombre = $"{n1} {n2} {a1}",
                                   Id = Guid.NewGuid().ToString(),
                                   CursoId = cursoid                                  
                              };

            return listaAlumnos.OrderBy((al) => al.Id).Take(cantidad).ToList();
        }
    }
}
