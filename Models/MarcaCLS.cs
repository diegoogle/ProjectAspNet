using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWeb.Models {
    public class MarcaCLS {
        //[@Tag]
        [Display(Name ="Id Marca")]//Se coloca Display para generar campo en la lista.
        public int iidmarca { get; set; }
        [Display(Name ="Nombre marca")]
        [Required]
        [StringLength(100, ErrorMessage = "Longitud maxima es 100")]
        public string nombre { get; set; }
        [Display(Name = "Descripcion marca")]
        [Required]
        [StringLength(200, ErrorMessage = "Longitud maxima es 200")]
        public string descripcion { get; set; }
        //[Display(Name = "")] //Si no se pone display no aparece.
        //[Display(Name ="Habilitado")] [Required] [StringLength(100)] Tambien se puede colocar asi
        public int bhabilitado { get; set; }
        //Añadiendo una propiedad
        public string mensajeError { get; set; }

    }
}