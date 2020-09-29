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
        public ActionResult Index(SucursalCLS sucursalCLS)
        {
            using (var bd = new BDPasajeEntities()) {
                if (sucursalCLS.nombre == null) {
                    listaSucursal = (from sucursal in bd.Sucursal
                                     where sucursal.BHABILITADO == 1
                                     select new SucursalCLS {
                                         iidsucursal = sucursal.IIDSUCURSAL,
                                         nombre = sucursal.NOMBRE,
                                         telefono = sucursal.TELEFONO,
                                         email = sucursal.EMAIL
                                     }).ToList();
                } else {
                    listaSucursal = (from sucursal in bd.Sucursal
                                     where sucursal.BHABILITADO == 1 
                                     && sucursal.NOMBRE.Contains(sucursalCLS.nombre)
                                     select new SucursalCLS {
                                         iidsucursal = sucursal.IIDSUCURSAL,
                                         nombre = sucursal.NOMBRE,
                                         telefono = sucursal.TELEFONO,
                                         email = sucursal.EMAIL
                                     }).ToList();
                }                
            }
                return View(listaSucursal);//Un view es HTML
        }
        public ActionResult Editar (int id) {
            SucursalCLS sucursalCLS = new SucursalCLS();
            using (var bd = new BDPasajeEntities()) {
                Sucursal sucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                sucursalCLS.iidsucursal = sucursal.IIDSUCURSAL;
                sucursalCLS.nombre = sucursal.NOMBRE;
                sucursalCLS.telefono = sucursal.TELEFONO;
                sucursalCLS.direccion = sucursal.DIRECCION;
                sucursalCLS.email = sucursal.EMAIL;
                sucursalCLS.fechaApertura = (DateTime)sucursal.FECHAAPERTURA;
            }
                return View(sucursalCLS);
        }
        public ActionResult Agregar () {
            return View();
        }
        [HttpPost]//Tag para post
        public ActionResult Agregar (SucursalCLS oSucursalCLS) {
            int nregistrosEncontrados = 0;
            using (var bd = new BDPasajeEntities()) {
                nregistrosEncontrados = bd.Sucursal.Where(p => p.NOMBRE.Equals(oSucursalCLS.nombre)).Count();
            }
            if (!ModelState.IsValid || nregistrosEncontrados > 0) {
;                if (nregistrosEncontrados > 0) oSucursalCLS.mensajeError = "Ya existe la sucursal";
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
        [HttpPost]
        public ActionResult Editar (SucursalCLS oSucursalCLS) {
            int nregistrosAfectados = 0;
            using (var bd = new BDPasajeEntities()) {
                nregistrosAfectados = bd.Sucursal.Where(p => p.NOMBRE.Equals(oSucursalCLS.nombre) && !p.IIDSUCURSAL.Equals(oSucursalCLS.iidsucursal)).Count();
            }
                int idSucursal = oSucursalCLS.iidsucursal;
            if (!ModelState.IsValid || nregistrosAfectados >0) {
                if (nregistrosAfectados > 0) oSucursalCLS.mensajeError = "Esta sucursal ya existe";
                return View(oSucursalCLS);
            }
            using (var bd = new BDPasajeEntities()) {
                Sucursal oSucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL == idSucursal).First();
                oSucursal.NOMBRE = oSucursalCLS.nombre;
                oSucursal.DIRECCION = oSucursalCLS.direccion;  
                oSucursal.TELEFONO = oSucursalCLS.telefono;
                oSucursal.EMAIL = oSucursalCLS.email;
                oSucursal.FECHAAPERTURA = oSucursalCLS.fechaApertura;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Eliminar(int id) {
            using (var bd = new BDPasajeEntities()) {
                Sucursal sucursal = bd.Sucursal.Where(p => p.IIDSUCURSAL.Equals(id)).First();
                sucursal.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
