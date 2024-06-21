using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElGatoMensajero.Models;

namespace ElGatoMensajero.Controllers
{
    public class PaquetesController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: Paquetes
        public ActionResult Index()
        {
            if (Session["user_id"] != null)
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                var paquetes = db.paquetes.Include(p => p.estado_paquetes).Include(p => p.personas).Include(p => p.personas1);
                return View(paquetes.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Paquetes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            paquetes paquete = db.paquetes.Find(id);

            if (paquete == null)
            {
                return HttpNotFound();
            }

            return View(paquete);
        }

        // GET: Paquetes/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor")
            {
                ViewBag.estado = new SelectList(db.estado_paquetes, "estado", "estado");
                return View();
            }
            else if (Session["user_id"] != null)
                return RedirectToAction("Index", "Paquetes");
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Paquetes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "peso,dimensiones,estado,emisor,receptor,nombreOrigen,nombreDestino,direccionOrigen,codPostalOrigen,tlDestino,direccionDestino,codPostalDestino, tlOrigen")] paquetes paquete)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor")
            {
                if (ModelState.IsValid)
                {
                    paquete.estado = "registrado";

                    Utilidades.Geocodificacion.guardarCoordenadasPaquetes(paquete);

                    db.paquetes.Add(paquete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.estado = new SelectList(db.estado_paquetes, "estado", "estado", paquete.estado);
                return View(paquete);
            }
            else if (Session["user_id"] != null)
                return RedirectToAction("Index", "Paquetes");
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Paquetes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                paquetes paquete = db.paquetes.Find(id);

                if (paquete == null)
                {
                    return HttpNotFound();
                }

                ViewBag.estado = new SelectList(db.estado_paquetes, "estado", "estado", paquete.estado);
                return View(paquete);
            }
            else if (Session["user_id"] != null)
                return RedirectToAction("Index", "Paquetes");
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Paquetes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,peso,dimensiones,estado,emisor,receptor,nombreOrigen,nombreDestino,direccionOrigen,codPostalOrigen,tlDestino,direccionDestino,codPostalDestino, tlOrigen")] paquetes paquete)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor")
            {
                if (ModelState.IsValid)
                {
                    Utilidades.Geocodificacion.guardarCoordenadasPaquetes(paquete);

                    db.Entry(paquete).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.estado = new SelectList(db.estado_paquetes, "estado", "estado", paquete.estado);
                return View(paquete);
            }
            else if (Session["user_id"] != null)
                return RedirectToAction("Index", "Paquetes");
            else
                return RedirectToAction("Index", "Home");
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Search()
        {
            return View();
        }

        // POST: Paquetes/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(paquetes paquete)
        {
            paquetes paq = db.paquetes.Find(paquete.id);

            if (paq != null)
            {
                return RedirectToAction("Details", "Paquetes", new { id = paq.id });
            }
            else
            {
                ViewBag.CustomError = "El paquete que intentas buscar no existe. Vuelve a intentarlo.";
                return View();
            }
        }

        public ActionResult Calculate()
        {
            asignarSedesPaquetes();
            string soluciones = "";
            for (int i = 0; i < db.sedes.Count<sedes>(); i++)
            {
                if ((db.sedes.OrderBy(o => o.id).Skip(i).First().latitud.ToString() != "") && (db.sedes.OrderBy(o => o.id).Skip(i).First().paquetes.Count > 0))
                {
                    soluciones += "Ruta desde la sede de " + db.sedes.OrderBy(o => o.id).Skip(i).First().localidad + " :\n\n";
                    soluciones += (Utilidades.Algoritmo.calcularRuta(db.sedes.OrderBy(o => o.id).Skip(i).First()));
                }
            }
            ViewBag.soluciones = soluciones;
            return View();
        }
        //método que asigna automáticamente los paquetes no asignados a ninguna sede a la que esté más cerca del remitente:
        private void asignarSedesPaquetes()
        {
            foreach (paquetes p in db.paquetes)
            {
                double minimo = Math.Abs(db.sedes.FirstOrDefault<sedes>().cp - (int)p.codPostalOrigen);
                int codigoMinimo = db.sedes.FirstOrDefault<sedes>().cp;
                if ((p.sedes.Count == 0)&&(p.latitud!=null))
                {
                    foreach (sedes s in db.sedes)
                    {
                        if (Math.Sqrt(Math.Pow((double)s.latitud - (double)p.latitud, 2) + Math.Pow((double)s.longitud - (double)p.longitud, 2)) < minimo)
                        {
                            minimo = Math.Sqrt(Math.Pow((double)s.latitud - (double)p.latitud, 2) + Math.Pow((double)s.longitud - (double)p.longitud, 2));
                            codigoMinimo = s.cp;
                        }
                    }
                    p.sedes = db.sedes.Where(s => s.cp == codigoMinimo).ToList();
                }
                //forma aproximada si no hubiera coordenadas:
                //suponemos que la diferencia entre los códigos postales es proporcional a la distancia:
                if (p.latitud == null) 
                {
                    minimo = Math.Abs(db.sedes.FirstOrDefault<sedes>().cp - (int)p.codPostalOrigen);
                    codigoMinimo = db.sedes.FirstOrDefault<sedes>().cp;
                    if (p.sedes.Count == 0)
                    {
                        foreach (sedes s in db.sedes)
                        {
                            if (Math.Abs(s.cp - (int)p.codPostalOrigen) < minimo)
                            {
                                minimo = Math.Abs(s.cp - (int)p.codPostalOrigen);
                                codigoMinimo = s.cp;
                            }
                        }
                        p.sedes = db.sedes.Where(s => s.cp == codigoMinimo).ToList();
                    }
                }
            }
            db.SaveChanges();
        }
    }
}