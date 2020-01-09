using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;
namespace MiPrimeraAplicacionWeb.Controllers
{
    public class EmpleadoController : Controller
    {
        // GET: Empleado
        public ActionResult Index()
        {
            List<EmpleadoCLS> listaEmpleados = null;
            using (var bd = new BDPasajeEntities()) {
                listaEmpleados = (from empleado in bd.Empleado
                                  join TipoUsuario in bd.TipoUsuario
                                  on empleado.IIDTIPOUSUARIO equals TipoUsuario.IIDTIPOUSUARIO
                                  join TipoContrato in bd.TipoContrato
                                  on empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                  where empleado.BHABILITADO == 1
                                  select new EmpleadoCLS {
                                      iidEmpleado = empleado.IIDEMPLEADO,
                                      nombre = empleado.NOMBRE,
                                      apPaterno = empleado.APPATERNO,
                                      nombreTipoUsuario = TipoUsuario.NOMBRE,
                                      nombreTipoContrato = TipoContrato.NOMBRE//Se debe inicializar con el nombre de la propiedad que se declaro en la clase modelo.
                                  }).ToList();
            }
                return View(listaEmpleados);
        }
        public void listarComboSexo () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from sexo in bd.Sexo
                         where sexo.BHABILITADO == 1
                         select new SelectListItem {
                             Text = sexo.NOMBRE,
                             Value = sexo.IIDSEXO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSexo = lista;
            }
        }
        public void listarTipoContrato () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.TipoContrato
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOCONTRATO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoContrato = lista;
            }
        }
        public void listarTipoUsuario () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.TipoUsuario
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,
                             Value = item.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoUsuario = lista;
            }
        }
        public void listarCombos () {
            listarComboSexo();
            listarTipoContrato();
            listarTipoUsuario();
        }
        public ActionResult Agregar () {
            return View();
        }
    }
}