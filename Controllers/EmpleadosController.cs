using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElGatoMensajero.Models;
using ElGatoMensajero.Utilidades;

namespace ElGatoMensajero.Controllers
{
    public class EmpleadosController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                var empleados = db.empleados.Include(e => e.personas).Include(e => e.sedes).Include(e => e.tipo_empleados).Include(e => e.personas.roles);
                return View(empleados.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario")
            {
                empleados emple;

                if (id != null)
                {
                    emple = db.empleados.Find(id);
                }
                else
                {
                    if (id == null && Session["user_id"] != null)
                    {
                        //Cargar perfil actual
                        emple = db.empleados.Find(Session["user_id"]);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }

                if (emple == null)
                {
                    return HttpNotFound();
                }

                return View(emple);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                ViewBag.sede = new SelectList(db.sedes, "id", "localidad");
                ViewBag.tipo = new SelectList(db.tipo_empleados, "tipo", "descripcion");
                ViewBag.rol = new SelectList(db.roles, "tipo", "tipo");

                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email, password, repeatPassword, nombre, apellidos, telefono, fecha_nacimiento, direccion, codPostal, rol")] personas persona, [Bind(Include = "dni,nss,fecha_inicio,fecha_fin,tipo,sede")] empleados empleado)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor" && Session["user_rol"].ToString() != "usuario")
            {
                enviarCorreo correo = new enviarCorreo();

                if (ModelState.IsValid)
                {
                    //Encriptar contraseña
                    db.personas.Add(persona);
                    db.SaveChanges();

                    empleado.id = persona.id;
                    db.empleados.Add(empleado);

                    db.SaveChanges();

                    string asunto = "Bienvenido al gato mensajero";
                    string cuerpo = "¡¡Felicidades!! Ha sido registrado correctamete como empleado de nuestra gran familis :').";

                    bool confirmacion = correo.SendMail(persona.email, asunto, cuerpo);

                    return RedirectToAction("Index", "Empleados");
                }

                empleado.personas = persona;

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad", empleado.sede);
                ViewBag.tipo = new SelectList(db.tipo_empleados, "tipo", "descripcion", empleado.tipo);
                ViewBag.rol = new SelectList(db.roles, "tipo", "tipo", persona.rol);

                return View("Create");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                empleados empleado = db.empleados.Find(id);

                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad", empleado.sede);
                ViewBag.tipo = new SelectList(db.tipo_empleados, "tipo", "descripcion", empleado.tipo);
                ViewBag.rol = new SelectList(db.roles, "tipo", "tipo", empleado.personas.rol);

                return View(empleado);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, email, password, repeatPassword, nombre, apellidos, telefono, fecha_nacimiento, direccion, codPostal, rol")] personas persona, [Bind(Include = "id, dni,nss,fecha_inicio,fecha_fin,tipo,sede")] empleados empleado)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario")
            {
                if (ModelState.IsValid)
                {
                    db.Entry(persona).State = EntityState.Modified;
                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                empleado.personas = persona;

                ViewBag.sede = new SelectList(db.sedes, "id", "localidad");
                ViewBag.tipo = new SelectList(db.tipo_empleados, "tipo", "descripcion");
                ViewBag.rol = new SelectList(db.roles, "tipo", "tipo");
                return View(empleado);
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
