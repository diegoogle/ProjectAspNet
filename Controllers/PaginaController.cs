using MiPrimeraAplicacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiPrimeraAplicacionWeb.Controllers
{
    public class PaginaController : Controller
    {
        // GET: Pagina
        public ActionResult Index()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities()) {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.BHABILITADO == 1
                               select new PaginaCLS {
                                   iidpagina = pagina.IIDPAGINA,
                                   mensaje = pagina.MENSAJE,
                                   controlador = pagina.CONTROLADOR,
                                   accion = pagina.ACCION
                               }).ToList();
            }
                return View(listaPagina);
        }
        public ActionResult Filtrar (PaginaCLS paginaCLS) {
            string mensaje = paginaCLS.mensaje;
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (var bd = new BDPasajeEntities()) {
                if (mensaje == null) {
                    
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   select new PaginaCLS {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                } else {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.BHABILITADO == 1
                                   && pagina.MENSAJE.Contains(mensaje)
                                   select new PaginaCLS {
                                       iidpagina = pagina.IIDPAGINA,
                                       mensaje = pagina.MENSAJE,
                                       controlador = pagina.CONTROLADOR,
                                       accion = pagina.ACCION
                                   }).ToList();
                }
            }
            return PartialView("_TablaPagina", listaPagina);
        }
        public int Guardar (PaginaCLS paginaCLS, int titulo) {
            int respuesta = 0;
            using (var bd = new BDPasajeEntities()) {
                if (titulo == 1) {
                    Pagina pagina = new Pagina();
                    pagina.MENSAJE = paginaCLS.mensaje;
                    pagina.ACCION = paginaCLS.accion;
                    pagina.CONTROLADOR = paginaCLS.controlador;
                    pagina.BHABILITADO = 1;
                    bd.Pagina.Add(pagina);
                    respuesta = bd.SaveChanges();
                }
            }
            return respuesta;
        }
    }
}