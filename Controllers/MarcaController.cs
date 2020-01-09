using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class MarcaController: Controller {
        // GET: Marca
        public ActionResult Index () {
            List<MarcaCLS> listaMarca = null;
            //Entity framework siempre se usa en los controladores
            using (var bd = new BDPasajeEntities()) { //Abre la conexion, con apertura y cierre automatico.
                listaMarca = (from marca in bd.Marca
                              where marca.BHABILITADO == 1
                              select new MarcaCLS {
                                  iidmarca = marca.IIDMARCA,
                                  nombre = marca.NOMBRE,
                                  descripcion = marca.DESCRIPCION
                              }).ToList();
            }
            return View(listaMarca);//Un view es HTML
        }
        public ActionResult Agregar () {//Este metodo crea la vista
            return View();
        }

        [HttpPost]
        public ActionResult Agregar (MarcaCLS oMarcaCLS) {//Este metodo ejecuta la insercion
            if (!ModelState.IsValid) {
                return View(oMarcaCLS);
            } else {
                using (var bd = new BDPasajeEntities()) {
                    Marca oMarca = new Marca();
                    oMarca.NOMBRE = oMarcaCLS.nombre;
                    oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                    oMarca.BHABILITADO = 1;
                    bd.Marca.Add(oMarca);
                    bd.SaveChanges();   
                }
                    
            }
            return RedirectToAction("Index");
        }
    }
}