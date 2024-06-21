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
    public class SedesController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: Sedes
        public ActionResult Index()
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() == "administrador" || Session["user_rol"].ToString() == "encargadoZona"))
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                return View(db.sedes.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Sedes/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                sedes sedes = db.sedes.Find(id);

                if (sedes == null)
                {
                    return HttpNotFound();
                }

                return View(sedes);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Sedes/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
                return View();
            else if (Session["user_id"] != null && Session["user_rol"].ToString() != "encargadoZona")
                return RedirectToAction("Index", "Sedes");
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Sedes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,provincia,localidad,cp,calle,num,telefono")] sedes sedes)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (ModelState.IsValid)
                {
                    Utilidades.Geocodificacion.guardarCoordenadasSedes(sedes);

                    db.sedes.Add(sedes);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(sedes);
            }
            else if (Session["user_id"] != null && Session["user_rol"].ToString() != "encargadoZona")
                return RedirectToAction("Index", "Sedes");
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Sedes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() == "administrador" || Session["user_rol"].ToString() == "encargadoZona"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                sedes sedes = db.sedes.Find(id);

                if (sedes == null)
                {
                    return HttpNotFound();
                }

                return View(sedes);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Sedes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,provincia,localidad,cp,calle,num,telefono")] sedes sedes)
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() == "administrador" || Session["user_rol"].ToString() == "encargadoZona"))
            {
                if (ModelState.IsValid)
                {
                    Utilidades.Geocodificacion.guardarCoordenadasSedes(sedes);

                    db.Entry(sedes).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(sedes);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Sedes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                sedes sede = db.sedes.Find(id);

                if (sede == null)
                {
                    return HttpNotFound();
                }

                if (sede.id == 1)
                {
                    TempData["CustomError"] = "No se puede borrar la sede por defecto.";
                    return RedirectToAction("Index");
                }

                db.sedes.Remove(sede);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else if (Session["user_id"] != null && Session["user_rol"].ToString() != "encargadoZona")
                return RedirectToAction("Index", "Sedes");
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