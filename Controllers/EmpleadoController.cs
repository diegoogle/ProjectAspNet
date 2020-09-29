using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;
namespace MiPrimeraAplicacionWeb.Controllers
{
    public class EmpleadoController: Controller {
        // GET: Empleado
        public ActionResult Index (EmpleadoCLS empleadoCLS) {
            listarTipoUsuario();
            List<EmpleadoCLS> listaEmpleados = null;
            using (var bd = new BDPasajeEntities()) {
                if (empleadoCLS.iidTipoUsuario == 0) {
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
                } else {
                    listaEmpleados = (from empleado in bd.Empleado
                                      join TipoUsuario in bd.TipoUsuario
                                      on empleado.IIDTIPOUSUARIO equals TipoUsuario.IIDTIPOUSUARIO
                                      join TipoContrato in bd.TipoContrato
                                      on empleado.IIDTIPOCONTRATO equals TipoContrato.IIDTIPOCONTRATO
                                      where empleado.BHABILITADO == 1
                                      && empleado.IIDTIPOUSUARIO == empleadoCLS.iidTipoUsuario
                                      select new EmpleadoCLS {
                                          iidEmpleado = empleado.IIDEMPLEADO,
                                          nombre = empleado.NOMBRE,
                                          apPaterno = empleado.APPATERNO,
                                          nombreTipoUsuario = TipoUsuario.NOMBRE,
                                          nombreTipoContrato = TipoContrato.NOMBRE//Se debe inicializar con el nombre de la propiedad que se declaro en la clase modelo.
                                      }).ToList();
                }
                
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
                             //Value = item.IIDTIPOUSUARIO
                             Value = item.IIDTIPOUSUARIO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoUsuario = lista;
            }
        }
        private string getString (int x) { return x.ToString(); }
        public void listarCombos () {
            listarComboSexo();
            listarTipoContrato();
            listarTipoUsuario();
        }
        public ActionResult Editar (int id) {
            listarCombos();
            EmpleadoCLS oEmpleadoCLS = new EmpleadoCLS();
            using (var bd = new BDPasajeEntities()) {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(id)).First();
                oEmpleadoCLS.iidEmpleado = oEmpleado.IIDEMPLEADO;
                oEmpleadoCLS.nombre = oEmpleado.NOMBRE;
                oEmpleadoCLS.apPaterno = oEmpleado.APPATERNO;
                oEmpleadoCLS.apMaterno = oEmpleado.APMATERNO;
                oEmpleadoCLS.fechaContrato = (DateTime)oEmpleado.FECHACONTRATO;
                oEmpleadoCLS.sueldo = (decimal)oEmpleado.SUELDO;
                oEmpleadoCLS.iidTipoUsuario = (int)oEmpleado.IIDTIPOUSUARIO;
                oEmpleadoCLS.iidTipoContrato = (int)oEmpleado.IIDTIPOCONTRATO;
                oEmpleadoCLS.iidSexo = (int)oEmpleado.IIDSEXO;

            }
            return View(oEmpleadoCLS);
        }

        [HttpPost]
        public ActionResult Editar (EmpleadoCLS oEmpleadoCLS) {
            int nRegistros = 0;
            using (var bd = new BDPasajeEntities()) {
                nRegistros = bd.Empleado.Where(p =>
                p.NOMBRE.Equals(oEmpleadoCLS.nombre)
                && p.APPATERNO.Equals(oEmpleadoCLS.apPaterno)
                && p.APMATERNO.Equals(oEmpleadoCLS.apMaterno) 
                && !p.IIDEMPLEADO.Equals(oEmpleadoCLS.iidEmpleado)).Count();
            }

            int idEmpleado = oEmpleadoCLS.iidEmpleado;

            if (!ModelState.IsValid || nRegistros > 0) {
                if (nRegistros > 0) oEmpleadoCLS.mensajeError = "Ya existe empleado";
                listarCombos();
                return View(oEmpleadoCLS);
            }
            using (var bd = new BDPasajeEntities()) {
                Empleado oEmpleado = bd.Empleado.Where(p => p.IIDEMPLEADO == idEmpleado).First();
                oEmpleado.NOMBRE = oEmpleadoCLS.nombre;
                oEmpleado.APPATERNO = oEmpleadoCLS.apPaterno;
                oEmpleado.APMATERNO = oEmpleadoCLS.apMaterno;                
                oEmpleado.FECHACONTRATO = oEmpleadoCLS.fechaContrato;
                oEmpleado.SUELDO = oEmpleadoCLS.sueldo;
                oEmpleado.IIDTIPOCONTRATO = oEmpleadoCLS.iidTipoContrato;
                oEmpleado.IIDTIPOUSUARIO = oEmpleadoCLS.iidTipoUsuario;                
                oEmpleado.IIDSEXO = oEmpleadoCLS.iidSexo;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Agregar () {
            listarCombos();
            return View();

        }
        [HttpPost]
        public ActionResult Agregar (EmpleadoCLS oempleadoCLS) {
            int nRegistros = 0;
            using (var bd = new BDPasajeEntities()) {
                nRegistros = bd.Empleado.Where(p => 
                p.NOMBRE.Equals(oempleadoCLS.nombre) &&
                p.APPATERNO.Equals(oempleadoCLS.apPaterno) && p.APMATERNO.Equals(oempleadoCLS.apMaterno)).Count();
            }
                if (!ModelState.IsValid || nRegistros >0) {
                if (nRegistros > 0) oempleadoCLS.mensajeError = "Empleado ya existe";
                    listarCombos();
                    return View(oempleadoCLS);
                }
            using (var bd = new BDPasajeEntities()) {
                Empleado oEmpleado = new Empleado();
                oEmpleado.NOMBRE = oempleadoCLS.nombre;
                oEmpleado.APPATERNO = oempleadoCLS.apPaterno;
                oEmpleado.APMATERNO = oempleadoCLS.apMaterno;
                oEmpleado.FECHACONTRATO = oempleadoCLS.fechaContrato;
                oEmpleado.SUELDO = oempleadoCLS.sueldo;
                oEmpleado.IIDTIPOUSUARIO = oempleadoCLS.iidTipoUsuario;
                oEmpleado.IIDTIPOCONTRATO = oempleadoCLS.iidTipoContrato;
                oEmpleado.IIDSEXO = oempleadoCLS.iidSexo;
                oEmpleado.BHABILITADO =1;
                bd.Empleado.Add(oEmpleado);
                bd.SaveChanges();
            }
            return RedirectToAction("Index");            
        }

        [HttpPost]
        public ActionResult Eliminar (int IdEmpleado) {
            using (var bd = new BDPasajeEntities()) {
                Empleado emp = bd.Empleado.Where(p => p.IIDEMPLEADO.Equals(IdEmpleado)).First();
                emp.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}