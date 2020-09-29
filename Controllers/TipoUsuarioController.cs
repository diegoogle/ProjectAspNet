using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;
namespace MiPrimeraAplicacionWeb.Controllers
{
    public class TipoUsuarioController : Controller
    {
        // GET: TipoUsuario
        private TipoUsuarioCLS tipoVal;
        private bool buscarTipoUsuario (TipoUsuarioCLS tipoUsuarioCLS) {

            bool busquedaID = true;
            bool busquedaNombre = true;
            bool busquedaDescripcion = true;

            if (tipoVal.iidtipousuario > 0) {
                busquedaID = tipoUsuarioCLS.iidtipousuario.ToString().Contains(tipoVal.iidtipousuario.ToString());
            }
            if (tipoVal.nombre != null) {
                busquedaNombre = tipoUsuarioCLS.nombre.ToString().Contains(tipoVal.nombre.ToString());
            }
            if (tipoVal.descripcion != null) {
                busquedaDescripcion = tipoUsuarioCLS.descripcion.ToString().Contains(tipoVal.descripcion.ToString());
            }
            return (busquedaID && busquedaNombre && busquedaDescripcion);
        }

        public ActionResult Index(TipoUsuarioCLS tipoUsuarioCLS)
        {
            tipoVal = tipoUsuarioCLS;
            List<TipoUsuarioCLS> listaTipoUsuario = null;
            List<TipoUsuarioCLS> listaFiltrado;
            using (var bd = new BDPasajeEntities()) {
                listaTipoUsuario = (from t in bd.TipoUsuario
                                    where t.BHABILITADO == 1
                                    select new TipoUsuarioCLS {
                                        iidtipousuario = t.IIDTIPOUSUARIO,
                                        nombre = t.NOMBRE,
                                        descripcion = t.DESCRIPCION
                                    }).ToList();
                if (tipoUsuarioCLS.iidtipousuario == 0 && tipoUsuarioCLS.nombre == null
                    && tipoUsuarioCLS.descripcion == null) {
                    listaFiltrado = listaTipoUsuario;
                } else {
                    Predicate<TipoUsuarioCLS> pred = new Predicate<TipoUsuarioCLS>(buscarTipoUsuario);
                    listaFiltrado = listaTipoUsuario.FindAll(pred);

                }
            }
                return View(listaFiltrado);
        }
    }
}