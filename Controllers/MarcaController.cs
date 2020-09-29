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
        public ActionResult Index (MarcaCLS marcaCLS) {//Busqueda en el index            
            List<MarcaCLS> listaMarca = null;            
            using (var bd = new BDPasajeEntities()) { 
                if (marcaCLS.nombre == null) {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1
                                  select new MarcaCLS {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                } else {
                    listaMarca = (from marca in bd.Marca
                                  where marca.BHABILITADO == 1 && marca.NOMBRE.Contains(marcaCLS.nombre)
                                  select new MarcaCLS {
                                      iidmarca = marca.IIDMARCA,
                                      nombre = marca.NOMBRE,
                                      descripcion = marca.DESCRIPCION
                                  }).ToList();
                }                
            }
            return View(listaMarca);
        }
        public ActionResult Agregar () {//Este metodo crea la vista
            return View();
        }

        public ActionResult Editar (int id) {
            MarcaCLS oMarcaCLS = new MarcaCLS();
            using (var bd = new BDPasajeEntities()) {                
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA.Equals(id)).First();
                oMarcaCLS.iidmarca = oMarca.IIDMARCA;
                oMarcaCLS.nombre = oMarca.NOMBRE;
                oMarcaCLS.descripcion = oMarca.DESCRIPCION;                
            }
                return View(oMarcaCLS);
        }

        [HttpPost]
        public ActionResult Editar (MarcaCLS oMarcaCLS) {
            int nregistrosEncontrados = 0;
            using (var bd = new BDPasajeEntities()) {
                nregistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(oMarcaCLS.nombre) && !p.IIDMARCA.Equals(oMarcaCLS.iidmarca)).Count();
            }
                if (!ModelState.IsValid || nregistrosEncontrados > 0) {
                if (nregistrosEncontrados > 0) oMarcaCLS.mensajeError = "Marca ya existe";
                    return View(oMarcaCLS);
                }
            int idMarca = oMarcaCLS.iidmarca;
            using (var bd = new BDPasajeEntities()) {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA == idMarca).First();
                oMarca.NOMBRE = oMarcaCLS.nombre;
                oMarca.DESCRIPCION = oMarcaCLS.descripcion;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Agregar (MarcaCLS oMarcaCLS) {//Este metodo ejecuta la insercion
            int nregistrosEncontrados = 0;
            string nombreMarca = oMarcaCLS.nombre;
            using (var bd = new BDPasajeEntities()) {
                nregistrosEncontrados = bd.Marca.Where(p => p.NOMBRE.Equals(nombreMarca)).Count();
            }
                if (!ModelState.IsValid || nregistrosEncontrados > 0) {
                if (nregistrosEncontrados > 0) oMarcaCLS.mensajeError = "El nombre marca ya existe";
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
        public ActionResult Eliminar (int id) {
            using (var bd = new BDPasajeEntities()) {
                Marca oMarca = bd.Marca.Where(p => p.IIDMARCA == id).First();
                oMarca.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}