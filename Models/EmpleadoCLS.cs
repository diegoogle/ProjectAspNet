﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace MiPrimeraAplicacionWeb.Models {
    public class EmpleadoCLS {

        [Display(Name ="Id Empleado")]        
        public int iidEmpleado { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage ="Longitud maxima 100")]
        //[MaxLength(100, ErrorMessage ="Maximo 100")]
        public string nombre { get; set; }

        [Display(Name = "Apellido Paterno")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]        
        public string apPaterno { get; set; }

        [Display(Name = "Apellido Materno")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima 200")]
        public string apMaterno { get; set; }

        [Display(Name = "Fecha Contrato")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//Formato de la fecha        
        public DateTime fechaContrato { get; set; }
        
        [Display(Name = "Tipo Usuario")]
        [Required]
        public int iidTipoUsuario { get; set; }

        [Display(Name = "Tipo Contrato")]
        [Required]
        public int iidTipoContrato { get; set; }

        [Display(Name = "Sexo")]
        [Required]
        public int iidSexo { get; set; }

        public int bhabilitado { get; set; }
        [Required]
        [Range(0,100000, ErrorMessage ="Fuera de rango")]
        [Display(Name ="Sueldo")]
        public decimal sueldo { get; set; }

        //Propiedades adicionales
        [Display(Name ="Tipo Contrato")]
        public string nombreTipoContrato { get; set; }

        [Display(Name ="Tipo Usuario")]
        public string nombreTipoUsuario { get; set; }
        public string mensajeError { get; set; }
    }
}