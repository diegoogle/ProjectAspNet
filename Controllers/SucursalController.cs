using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;
namespace MiPrimeraAplicacionWeb.Controllers
{
    public class SucursalController : Controller
    {
        // GET: Sucursal
        List<SucursalCLS> listaSucursal = null;
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities()) {
                listaSucursal = (from sucursal in bd.Sucursal
                                 where sucursal.BHABILITADO==1
                                 select new SucursalCLS {
                                     iidsucursal = sucursal.IIDSUCURSAL,
                                     nombre = sucursal.NOMBRE,
                                     telefono = sucursal.TELEFONO,
                                     email = sucursal.EMAIL
                                 }).ToList();
            }
                return View(listaSucursal);//Un view es HTML
        }
        public ActionResult Agregar () {
            return View();
        }
        [HttpPost]//Tag para post
        public ActionResult Agregar (SucursalCLS oSucursalCLS) {
            if (!ModelState.IsValid) {
                return View(oSucursalCLS);
            }
            using (var bd = new BDPasajeEntities()) {
                Sucursal oSucursal = new Sucursal();
                oSucursal.NOMBRE = oSucursalCLS.nombre;
                oSucursal.DIRECCION = oSucursalCLS.direccion;
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;
                oSucursal.BHABILITADO = 1;
                bd.Sucursal.Add(oSucursal);
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
        }
    }
}
