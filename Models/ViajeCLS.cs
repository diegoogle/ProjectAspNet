using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiPrimeraAplicacionWeb.Models {
    public class ViajeCLS {
        [Display(Name = "Id Viaje")]
        public int iidViaje { get; set; }

        [Display(Name = "Lugar Origen")]
        [Required]
        public int iidLugarOrigen { get; set; }

        [Display(Name = "Lugar Destino")]
        [Required]
        public int iidLugarDestino { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [Range(0, 100000, ErrorMessage ="Precio incorrecto")]
        public double precio { get; set; }

        [Display(Name = "Fecha Viaje")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//Formato de la fecha        
        public DateTime fechaViaje { get; set; }

        [Display(Name = "Bus")]
        [Required]
        public int iidBus { get; set; }

        [Display(Name = "Numero Asientos Disponibles")]
        [Required]
        public int numeroAsientosDisponibles { get; set; }

        //Propiedades adicionales
        [Display(Name = "Lugar Origen")]
        public string nombreLugarOrigen { get; set; }

        [Display(Name = "Lugar Destino")]
        public string nombreLugarDestino { get; set; }

        [Display(Name = "Bus")]
        public string nombreBus { get; set; }
    }
}