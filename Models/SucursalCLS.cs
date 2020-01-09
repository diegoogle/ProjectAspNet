using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWeb.Models {
    public class SucursalCLS {

        [Display(Name = "Id Sucursal")]//Tag
        public int iidsucursal { get; set; }
        [Required][Display(Name ="Nombre Sucursal")][StringLength(100, ErrorMessage ="Longitud maxima 100")]
        public string nombre { get; set; }        
        [Required][Display(Name ="Direccion")][StringLength(100, ErrorMessage ="Longitud maxima 100")]
        public string direccion { get; set; }
        [Required][Display(Name = "Telefono Sucursal")]
        public string telefono { get; set; }
        [Required][Display(Name = "Email Sucursal")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string email { get; set; }

        [Required]//Campo requerido
        [Display(Name ="Fecha Apertura")]//Nombre del campo
        [DataType(DataType.Date)]//Controlador de fecha         
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]//Formato de la fecha        
        public DateTime fechaApertura  { get; set; }
        public int bhabilitado { get; set; }
    }
}