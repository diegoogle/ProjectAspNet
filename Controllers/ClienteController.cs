using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using MiPrimeraAplicacionWeb.Models;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class ClienteController : Controller
    {
        public List<ClienteCLS> listaCliente = null;
        // GET: Cliente
        public ActionResult Index()
        {
            using (var bd = new BDPasajeEntities()) {
                listaCliente = (from cliente in bd.Cliente
                                where cliente.BHABILITADO == 1
                                select new ClienteCLS {
                                    iidcliente = cliente.IIDCLIENTE,
                                    nombre = cliente.NOMBRE,
                                    apPaterno = cliente.APPATERNO,
                                    apMaterno = cliente.APMATERNO,
                                    email = cliente.EMAIL,
                                    direccion = cliente.DIRECCION,
                                    iidsexo = (int)cliente.IIDSEXO,
                                    telefonoFijo = cliente.TELEFONOFIJO,
                                    telefonoCelular = cliente.TELEFONOCELULAR                                    
                                }).ToList();
            }
                return View(listaCliente);
        }
        List<SelectListItem> listaSexos;
        private void llenarListaSexos () {
            using (var bd = new BDPasajeEntities()) {
                listaSexos = (from sexo in bd.Sexo
                              where sexo.BHABILITADO == 1
                              select new SelectListItem {
                                  Text = sexo.NOMBRE,                                  
                                  Value = SqlFunctions.StringConvert((double)sexo.IIDSEXO)
                              }).ToList();
                listaSexos.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }
        public ActionResult Agregar () {
            llenarListaSexos();
            ViewBag.lista = listaSexos;
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS) {
            if (!ModelState.IsValid) {
                llenarListaSexos();
                ViewBag.lista = listaSexos;//Se repite el llenado de combo box para que no caiga en error
                return View(oClienteCLS);
            } else { 
            using (var bd = new BDPasajeEntities()) {
                Cliente oCliente = new Cliente();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apPaterno;
                oCliente.APMATERNO = oClienteCLS.apMaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                oCliente.BHABILITADO = 1;
                bd.Cliente.Add(oCliente);
                bd.SaveChanges();
            }
                return RedirectToAction("Index");
            }
        }
    }
}