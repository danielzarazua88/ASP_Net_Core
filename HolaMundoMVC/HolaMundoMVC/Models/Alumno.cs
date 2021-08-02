using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HolaMundoMVC.Models
{
    public class Alumno: ObjetoEscuelaBase
    {
        [Required]
        public string CursoId { get; set; }
        public Curso Curso { get; set; }
        public List<Evaluacion> Evaluaciones { get; set; } 
    }
}