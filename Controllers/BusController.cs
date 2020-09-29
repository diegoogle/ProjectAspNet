using MiPrimeraAplicacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class BusController : Controller
    {
        [HttpPost]
        public ActionResult Eliminar (int iidBus) {
            using (var bd = new BDPasajeEntities()) {
                Bus oBus = bd.Bus.Where(p => p.IIDBUS.Equals(iidBus)).First();
                oBus.BHABILITADO = 0;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public void listarCombos () {
            listarTipoBus();
            listarSucursal();
            listarModelo();
            listarMarca();
        }
        public ActionResult Editar (int id) {
            BusCLS oBusCls = new BusCLS();
            listarCombos();
            using (var bd = new BDPasajeEntities()) {//No regresa informacion
                Bus oBus = bd.Bus.Where(p => p.IIDBUS.Equals(id)).First();
                oBusCls.iidBus = oBus.IIDBUS;
                oBusCls.iidSucursal = (int)oBus.IIDSUCURSAL;
                oBusCls.iidTipoBus = (int)oBus.IIDTIPOBUS;
                oBusCls.iidModelo = (int)oBus.IIDMODELO;
                oBusCls.iidMarca = (int)oBus.IIDMARCA;
                oBusCls.placa = oBus.PLACA;
                oBusCls.fechaCompra = (DateTime)oBus.FECHACOMPRA;                
                oBusCls.numeroColumnas = (int)oBus.NUMEROCOLUMNAS;
                oBusCls.numeroFilas = (int)oBus.NUMEROFILAS;
                oBusCls.descripcion = oBus.DESCRIPCION;
                oBusCls.observacion = oBus.OBSERVACION;                

            }
                return View(oBusCls);
        }
        [HttpPost]
        public ActionResult Editar (BusCLS oBusCLS) {
            int nRegistros = 0;
            int idBus = oBusCLS.iidBus;
            string placa = oBusCLS.placa;

            using (var bd = new BDPasajeEntities()) {
                nRegistros = bd.Bus.Where(p => p.PLACA.Equals(placa) && !p.IIDBUS.Equals(idBus)).Count();
            }
            if (!ModelState.IsValid || nRegistros > 0) {
                if (nRegistros > 0) oBusCLS.mensajeError = "El Bus ya existe";
                listarCombos();
                return View(oBusCLS);
            }
            
            using (var bd = new BDPasajeEntities()) {
                Bus oBus = bd.Bus.Where(p => p.IIDBUS == idBus).First();
                oBus.IIDSUCURSAL = oBusCLS.iidSucursal;
                oBus.IIDMODELO = oBusCLS.iidModelo;
                oBus.IIDMARCA = oBusCLS.iidMarca;
                oBus.IIDTIPOBUS = oBusCLS.iidTipoBus;
                oBus.FECHACOMPRA = oBusCLS.fechaCompra;
                oBus.DESCRIPCION = oBusCLS.descripcion;
                oBus.PLACA = oBusCLS.placa;
                oBus.NUMEROFILAS = oBusCLS.numeroFilas;
                oBus.NUMEROCOLUMNAS = oBusCLS.numeroColumnas;
                oBus.DESCRIPCION = oBusCLS.descripcion;
                oBus.OBSERVACION = oBusCLS.observacion;
                bd.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Agregar () {
            listarCombos();
            return View();
        }
        [HttpPost]
        public ActionResult Agregar (BusCLS oBusCLS) {

            int nRegistros = 0;
            string placa = oBusCLS.placa;
            
            using (var bd = new BDPasajeEntities()) {
                
                nRegistros = bd.Bus.Where(p => p.PLACA.Equals(placa)).Count();
            }
                if (!ModelState.IsValid || nRegistros > 0) {
                if (nRegistros > 0) oBusCLS.mensajeError = "Ya Existe el Bus";
                    listarCombos();
                    return View(oBusCLS);
                }
            using (var bd = new BDPasajeEntities()) {
                Bus oBus = new Bus();
                oBus.IIDSUCURSAL = oBusCLS.iidSucursal;                
                oBus.IIDMARCA = oBusCLS.iidMarca;                
                oBus.IIDMODELO = oBusCLS.iidModelo;                
                oBus.IIDTIPOBUS = oBusCLS.iidTipoBus;  
                oBus.PLACA = oBusCLS.placa;
                oBus.FECHACOMPRA = oBusCLS.fechaCompra;
                oBus.NUMEROCOLUMNAS = oBusCLS.numeroColumnas;
                oBus.NUMEROFILAS = oBusCLS.numeroFilas;
                oBus.DESCRIPCION = oBusCLS.descripcion;
                oBus.OBSERVACION = oBusCLS.observacion;                
                oBus.BHABILITADO = 1;
                bd.Bus.Add(oBus);
                bd.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        // GET: Bus
        public ActionResult Index(BusCLS busCLS)
        {
            listarCombos();
            List<BusCLS> listaBus = null;
            List<BusCLS> listaRespuesta = new List<BusCLS>();
            using (var bd = new BDPasajeEntities()) {
                listaBus = (from bus in bd.Bus
                            join sucursal in bd.Sucursal
                            on bus.IIDSUCURSAL equals sucursal.IIDSUCURSAL
                            join tipoBus in bd.TipoBus
                            on bus.IIDTIPOBUS equals tipoBus.IIDTIPOBUS
                            join tipoModelo in bd.Modelo
                            on bus.IIDMODELO equals tipoModelo.IIDMODELO
                            where bus.BHABILITADO == 1
                            select new BusCLS {
                                iidBus = bus.IIDBUS,
                                placa = bus.PLACA,
                                nombreModelo = tipoModelo.NOMBRE,
                                nombreSucursal = sucursal.NOMBRE,
                                nombreTipoBus = tipoBus.NOMBRE,
                                iidModelo = tipoModelo.IIDMODELO,
                                iidSucursal = sucursal.IIDSUCURSAL,
                                iidTipoBus = tipoBus.IIDTIPOBUS
                            }
                               ).ToList();
                if (busCLS.iidBus == 0
                && busCLS.placa == null
                && busCLS.iidModelo == 0
                && busCLS.iidSucursal == 0
                && busCLS.iidTipoBus == 0) {
                    listaRespuesta = listaBus;
                } else {
                    //Filtros
                    if (busCLS.iidBus != 0) {
                        listaBus = listaBus.Where(p => p.iidBus.ToString().Contains(busCLS.iidBus.ToString())).ToList();
                    }
                    if (busCLS.placa != null) {
                        listaBus = listaBus.Where(p => p.placa.Contains(busCLS.placa)).ToList();
                    }
                    if (busCLS.iidModelo != 0) {
                        listaBus = listaBus.Where(p => p.iidModelo.ToString().Contains(busCLS.iidModelo.ToString())).ToList();
                    }
                    if (busCLS.iidSucursal != 0) {
                        listaBus = listaBus.Where(p => p.iidSucursal.ToString().Contains(busCLS.iidSucursal.ToString())).ToList();
                    }
                    if (busCLS.iidTipoBus != 0) {
                        listaBus = listaBus.Where(p => p.iidTipoBus.ToString().Contains(busCLS.iidTipoBus.ToString())).ToList();
                    }
                    listaRespuesta = listaBus;
                }
            }
            return View(listaRespuesta);
        }
        public void listarTipoBus () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.TipoBus
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,                             
                             Value = item.IIDTIPOBUS.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaTipoBus = lista;
            }
        }
        public void listarMarca () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.Marca
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,                            
                             Value = item.IIDMARCA.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaMarca = lista;
            }
        }
        public void listarModelo () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.Modelo
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,                             
                             Value = item.IIDMODELO.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaModelo = lista;
            }
        }
        public void listarSucursal () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from item in bd.Sucursal
                         where item.BHABILITADO == 1
                         select new SelectListItem {
                             Text = item.NOMBRE,                             
                             Value = item.IIDSUCURSAL.ToString()
                         }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaSucursal = lista;
            }
        }        
    }
}