using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MiPrimeraAplicacionWeb.Models;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class ClienteController : Controller
    {
        public List<ClienteCLS> listaCliente = null;
        // GET: Cliente
        public ActionResult Index(ClienteCLS clienteCLS)
        {
            llenarListaSexos();
            ViewBag.lista = listaSexo;
            using (var bd = new BDPasajeEntities()) {
                if (clienteCLS.iidsexo == 0) {
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
                } else {
                    listaCliente = (from cliente in bd.Cliente
                                    where cliente.BHABILITADO == 1
                                    && cliente.IIDSEXO == clienteCLS.iidsexo
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
            }
                return View(listaCliente);
        }
        List<SelectListItem> listaSexo;
        private void llenarListaSexos () {
            using (var bd = new BDPasajeEntities()) {
                listaSexo = (from sexo in bd.Sexo
                              where sexo.BHABILITADO == 1
                              select new SelectListItem {
                                  Text = sexo.NOMBRE,                                  
                                  Value = sexo.IIDSEXO.ToString()
                              }).ToList();
                listaSexo.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
            }
        }
        public ActionResult Agregar () {
            llenarListaSexos();
            ViewBag.lista = listaSexo;
            return View();
        }       
        public ActionResult Editar (int id) {
            ClienteCLS oClienteCLS = new ClienteCLS();
            using (var bd = new BDPasajeEntities()) {
                llenarListaSexos();
                ViewBag.lista = listaSexo;
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(id)).First();
                oClienteCLS.iidcliente = oCliente.IIDCLIENTE;
                oClienteCLS.nombre = oCliente.NOMBRE;
                oClienteCLS.apPaterno = oCliente.APPATERNO;
                oClienteCLS.apMaterno = oCliente.APMATERNO;
                oClienteCLS.direccion = oCliente.DIRECCION;
                oClienteCLS.email = oCliente.EMAIL;
                oClienteCLS.iidsexo = (int)oCliente.IIDSEXO;
                oClienteCLS.telefonoCelular = oCliente.TELEFONOCELULAR;
                oClienteCLS.telefonoFijo = oCliente.TELEFONOFIJO;
            }
                return View(oClienteCLS);
        }

        [HttpPost]
        public ActionResult Editar (ClienteCLS oClienteCLS) {
            int nRegistros = 0;
            using (var bd = new BDPasajeEntities()) {
                nRegistros = bd.Cliente.Where(p => p.NOMBRE.Equals(oClienteCLS.nombre) && p.APMATERNO.Equals(oClienteCLS.apMaterno) &&
                p.APPATERNO.Equals(oClienteCLS.apPaterno) && !p.IIDCLIENTE.Equals(oClienteCLS.iidcliente)).Count();
            }
                if (!ModelState.IsValid || nRegistros > 0) {
                if (nRegistros > 0) oClienteCLS.mensajeError = "Ya existe un cliente similar";
                llenarListaSexos();
                return View(oClienteCLS);
                }
            int idCliente = oClienteCLS.iidcliente;
            using (var bd = new BDPasajeEntities()) {
                Cliente oCliente = bd.Cliente.Where(p => p.IIDCLIENTE == idCliente).First();
                oCliente.NOMBRE = oClienteCLS.nombre;
                oCliente.APPATERNO = oClienteCLS.apPaterno;
                oCliente.APMATERNO = oClienteCLS.apMaterno;
                oCliente.EMAIL = oClienteCLS.email;
                oCliente.DIRECCION = oClienteCLS.direccion;
                oCliente.IIDSEXO = oClienteCLS.iidsexo;
                oCliente.TELEFONOFIJO = oClienteCLS.telefonoFijo;
                oCliente.TELEFONOCELULAR = oClienteCLS.telefonoCelular;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Agregar(ClienteCLS oClienteCLS) {
            int numRegistros = 0;
            using (var bd = new BDPasajeEntities()) {
                numRegistros = bd.Cliente.Where(p => p.NOMBRE.Equals(oClienteCLS.nombre) && p.APPATERNO.Equals(oClienteCLS.apPaterno)
                && p.APMATERNO.Equals(oClienteCLS.apMaterno)).Count();
            }
                if (!ModelState.IsValid || numRegistros > 0) {
                if (numRegistros > 0) oClienteCLS.mensajeError = "Ya existe este cliente registrado";
                    llenarListaSexos();
                    ViewBag.lista = listaSexo;//Se repite el llenado de combo box para que no caiga en error
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
        public ActionResult Eliminar(int iidcliente) {
            using (var bd = new BDPasajeEntities()) {
                Cliente cliente = bd.Cliente.Where(p => p.IIDCLIENTE.Equals(iidcliente)).First();
                cliente.BHABILITADO = 0;
                bd.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
    }
}