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
    public class TransportesController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: Transportes
        public ActionResult Index()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                var transportes = db.transportes.Include(t => t.sedes);

                return View(transportes.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Transportes/Details/5
        public ActionResult Details(string id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                transportes transporte = db.transportes.Find(id);

                if (transporte == null)
                {
                    return HttpNotFound();
                }

                return View(transporte);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Transportes/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                ViewBag.sede = new SelectList(db.sedes, "id", "localidad");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Transportes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "matricula,marca,modelo,consumo,km,fecha_matriculacion,sede")] transportes transporte)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (ModelState.IsValid)
                {
                    db.transportes.Add(transporte);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad", transporte.sede);

                return View(transporte);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Transportes/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                transportes transporte = db.transportes.Find(id);

                if (transporte == null)
                {
                    return HttpNotFound();
                }

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad", transporte.sede);

                return View(transporte);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Transportes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "matricula,marca,modelo,consumo,km,fecha_matriculacion,sede")] transportes transporte)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(transporte).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad", transporte.sede);

                return View(transporte);
            }
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
    }
}
