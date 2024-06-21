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
    public class UsuariosController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        // GET: Usuarios
        public ActionResult Index()
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "usuario")
            {
                if (TempData["CustomError"] != null)
                {
                    ViewBag.CustomError = TempData["CustomError"];
                    TempData.Remove("CustomError");
                }

                var usuarios = db.usuarios.Include(u => u.personas);
                return View(usuarios.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["user_id"] != null)
            {
                usuarios user;

                if (id != null)
                {
                    user = db.usuarios.Find(id);
                }
                else
                {
                    if (Session["user_id"] != null)
                    {
                        //Cargar perfil actual
                        user = db.usuarios.Find(Session["user_id"]);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }

                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor"))
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username")] usuarios usuario, [Bind(Include = "email, password, repeatPassword, nombre, apellidos, telefono, fecha_nacimiento, direccion, codPostal")] personas persona)
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor"))
            {
                if (ModelState.IsValid)
                {
                    enviarCorreo correo = new enviarCorreo();
                    
                    persona.rol = "usuario";

                    //Encriptar contraseña

                    db.personas.Add(persona);
                    db.SaveChanges();
                    usuario.id = persona.id;

                    db.usuarios.Add(usuario);
                    db.SaveChanges();

                    string asunto = "Bienvenido al gato mensajero";
                    string cuerpo = "¡¡Felicidades!! Nuestros técnicos le han creado la cuenta correctamente.";

                    bool confirmacion = correo.SendMail(persona.email, asunto, cuerpo);

                    return RedirectToAction("Index");
                }
            
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                usuarios usuario = db.usuarios.Find(id);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                return View(usuario);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, username")] usuarios usuario, [Bind(Include = "id, email, password, repeatPassword, nombre, apellidos, telefono, fecha_nacimiento, direccion, codPostal")] personas persona)
        {
            if (Session["user_id"] != null && Session["user_rol"].ToString() != "repartidor")
            {
                if (ModelState.IsValid)
                {
                    persona.rol = "usuario";

                    db.Entry(persona).State = EntityState.Modified;
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                usuario.personas = persona;

                return View(usuario);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["user_id"] != null && (Session["user_rol"].ToString() != "usuario" && Session["user_rol"].ToString() != "repartidor"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                usuarios usuario = db.usuarios.Find(id);
                personas persona = db.personas.Find(usuario.id);

                if (usuario == null)
                {
                    return HttpNotFound();
                }

                db.usuarios.Remove(usuario);
                db.personas.Remove(persona);
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
