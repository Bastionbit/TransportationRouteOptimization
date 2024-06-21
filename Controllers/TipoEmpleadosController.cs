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
    public class TipoEmpleadosController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: TipoEmpleados
        public ActionResult Index()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                return View(db.tipo_empleados.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: TipoEmpleados/Details/5
        public ActionResult Details(short? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                tipo_empleados tipo_empleados = db.tipo_empleados.Find(id);

                if (tipo_empleados == null)
                {
                    return HttpNotFound();
                }

                return View(tipo_empleados);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: TipoEmpleados/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: TipoEmpleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "salario,descripcion")] tipo_empleados tipo_empleado)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (ModelState.IsValid)
                {
                    db.tipo_empleados.Add(tipo_empleado);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(tipo_empleado);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: TipoEmpleados/Edit/5
        public ActionResult Edit(short? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                tipo_empleados tipo_empleados = db.tipo_empleados.Find(id);

                if (tipo_empleados == null)
                {
                    return HttpNotFound();
                }

                return View(tipo_empleados);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: TipoEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tipo, salario, descripcion")] tipo_empleados tipo_empleado)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(tipo_empleado).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(tipo_empleado);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: TipoEmpleados/Delete/5
        public ActionResult Delete(short? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() == "administrador")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                tipo_empleados tipo_empleado = db.tipo_empleados.Find(id);

                if (tipo_empleado == null)
                {
                    return HttpNotFound();
                }

                if (tipo_empleado.tipo == 1)
                {
                    TempData["CustomError"] = "No se puede borrar el tipo por defecto.";
                    return RedirectToAction("Index");
                }

                db.tipo_empleados.Remove(tipo_empleado);
                db.SaveChanges();

                return RedirectToAction("Index");
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
