using MiPrimeraAplicacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class ViajeController : Controller
    {

        // GET: Viaje        
        private void listarLugar () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from lugar in bd.Lugar
                             where lugar.BHABILITADO == 1
                             select new SelectListItem {
                                 Text = lugar.NOMBRE,
                                 Value = lugar.IIDLUGAR.ToString()
                             }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaLugar = lista;
            }
        }        
        private void listarBus () {
            List<SelectListItem> lista;
            using (var bd = new BDPasajeEntities()) {
                lista = (from Bus in bd.Bus
                              where Bus.BHABILITADO == 1
                              select new SelectListItem {
                                  Text = Bus.PLACA,
                                  Value = Bus.IIDBUS.ToString()
                              }).ToList();
                lista.Insert(0, new SelectListItem { Text = "--Seleccione--", Value = "" });
                ViewBag.listaBus = lista;
            }
        }
        public void listarCombos () {
            listarBus();
            listarLugar();
        }
        public ActionResult Index()
        {
            List<ViajeCLS> listaViaje = null;
            using (var bd = new BDPasajeEntities()) {
                listaViaje = (from viaje in bd.Viaje
                              join lugarOrigen in bd.Lugar
                              on viaje.IIDLUGARORIGEN equals lugarOrigen.IIDLUGAR
                              join lugarDestino in bd.Lugar
                              on viaje.IIDLUGARDESTINO equals lugarDestino.IIDLUGAR
                              join bus in bd.Bus
                              on viaje.IIDBUS equals bus.IIDBUS
                              select new ViajeCLS {
                                  iidViaje = viaje.IIDVIAJE,
                                  nombreBus = bus.PLACA,
                                  nombreLugarOrigen = lugarOrigen.NOMBRE,
                                  nombreLugarDestino = lugarDestino.NOMBRE
                              }).ToList();
            }
                return View(listaViaje);
        }
        public ActionResult Agregar () {
            listarCombos();
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(ViajeCLS oViajeCLS) {
            if (!ModelState.IsValid) {
                listarCombos();
                return View(oViajeCLS);
            } else {
                using (var bd = new BDPasajeEntities()) {
                    Viaje oViaje = new Viaje();
                    oViaje.IIDBUS = oViajeCLS.iidBus;
                    oViaje.IIDLUGARDESTINO = oViajeCLS.iidLugarDestino;
                    oViaje.IIDLUGARORIGEN = oViajeCLS.iidLugarOrigen;
                    oViaje.PRECIO = (decimal)oViajeCLS.precio;
                    oViaje.NUMEROASIENTOSDISPONIBLES = oViajeCLS.numeroAsientosDisponibles;
                    oViaje.FECHAVIAJE = oViajeCLS.fechaViaje;
                    bd.SaveChanges();
                }
                return RedirectToAction("Index");
            }                        
        }
    }
}