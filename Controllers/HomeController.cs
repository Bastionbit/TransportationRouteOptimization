using System.Web.Mvc;
using ElGatoMensajero.Models;
using ElGatoMensajero.Utilidades;
using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data.Entity;

namespace ElGatoMensajero.Controllers
{
    public class HomeController : Controller
    {
        private elGatoMensajeroEntities db = new elGatoMensajeroEntities();

        public ActionResult Index()
        {
            if (TempData["CustomError"] != null)
            {
                ViewBag.CustomError = TempData["CustomError"];
                TempData.Remove("CustomError");
            }

            return View();
        }

        public ActionResult Login()
        {
            if (Session["user_id"] == null)
                return View("Login");
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "email, password")] personas persona)
        {
            if (Session["user_id"] == null)
            {
                int dateComparation;
                string email = persona.email;
                string parol;
                string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["alternativa"].ConnectionString;
                SqlConnection conexion = new SqlConnection(cadena);
                SqlCommand consulta1;
                SqlDataReader existe;
                empleados empleado;

                conexion.Open();

                consulta1 = new SqlCommand("select id, email, password, nombre, apellidos, rol from personas where email='" + email + "'", conexion);

                try
                {
                    consulta1.ExecuteNonQuery();
                    existe = consulta1.ExecuteReader();

                    if (existe.Read())
                    {
                        parol = persona.password;

                        if (parol == existe["password"].ToString())
                        {
                            try
                            {
                                empleado = db.empleados.Find(existe["id"]);

                                if (empleado == null || empleado.fecha_fin == null)
                                {
                                    CreateSession(int.Parse(existe["id"].ToString()), existe["email"].ToString(), existe["nombre"].ToString(), existe["apellidos"].ToString(), existe["rol"].ToString());

                                    //Redireccionar a un sitio dependiendo del rol
                                    switch (existe["rol"].ToString())
                                    {
                                        case "usuario":
                                            return RedirectToAction("Details", "Usuarios");
                                        case "repartidor":
                                            return RedirectToAction("Details", "Empleados");
                                        case "encargadoSede":
                                            return RedirectToAction("Index", "Empleados");
                                        case "encargadoZona":
                                            return RedirectToAction("Index", "Sedes");
                                        default:
                                            return RedirectToAction("Index");
                                    }
                                }
                                else
                                {
                                    dateComparation = DateTime.Compare(Convert.ToDateTime(empleado.fecha_fin), DateTime.Now);

                                    if (dateComparation > 0)
                                    {
                                        CreateSession(int.Parse(existe["id"].ToString()), existe["email"].ToString(), existe["nombre"].ToString(), existe["apellidos"].ToString(), existe["rol"].ToString());

                                        //Redireccionar a un sitio dependiendo del rol
                                        switch (existe["rol"].ToString())
                                        {
                                            case "usuario":
                                                return RedirectToAction("Details", "Usuarios");
                                            case "repartidor":
                                                return RedirectToAction("Details", "Empleados");
                                            case "encargadoSede":
                                                return RedirectToAction("Index", "Empleados");
                                            case "encargadoZona":
                                                return RedirectToAction("Index", "Sedes");
                                            default:
                                                return RedirectToAction("Index");
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.CustomError = "No se ha podido iniciar sesión. Tu contrato ha finalizado.";
                                        return View("Login");
                                    }
                                }
                            }
                            catch
                            {
                                ViewBag.CustomError = "No se ha podido iniciar sesión. Si sigue ocurriendo el error contacte con un administrador.";
                                return View("Login");
                            }
                        }
                        else
                        {
                            ViewBag.CustomError = "No se ha podido iniciar sesión. La contraseña introducida no es correcta. Vuelve a intentarlo.";
                            return View("Login");
                        }
                    }
                    else
                    {
                        ViewBag.CustomError = "No se ha podido iniciar sesión. El correo introducido no es correcto.";
                        return View("Login");
                    }
                }
                catch
                {
                    ViewBag.CustomError = "No se ha podido iniciar sesión. Si sigue ocurriendo el error contacte con un administrador.";
                    return View("Login");
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult Register()
        {
            if (Session["user_id"] == null)
                return View("Register");
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "username")] usuarios usuario, [Bind(Include = "email, password, repeatPassword, nombre, apellidos, telefono, fecha_nacimiento, direccion, codPostal")] personas persona)
        {
            enviarCorreo correo = new enviarCorreo();

            if (Session["user_id"] == null)
            {
                if (ModelState.IsValid)
                {
                    //Cargar roles
                    persona.rol = "usuario";

                    //Encriptar contraseña
                    /*
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(persona.password);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    String hash = System.Text.Encoding.ASCII.GetString(data);
                    persona.password = hash;*/

                    db.personas.Add(persona);
                    db.SaveChanges();
                    usuario.id = persona.id;

                    db.usuarios.Add(usuario);
                    db.SaveChanges();

                    string asunto = "Bienvenido al gato mensajero";
                    string cuerpo = "¡¡Felicidades!! Su registro se ha completado correctamente.";

                    bool confirmacion = correo.SendMail(persona.email, asunto, cuerpo);

                    CreateSession(persona.id, persona.email, persona.nombre, persona.apellidos, persona.rol);

                    return RedirectToAction("Details", "Usuarios");
                }

                return View("Register");
            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult RecuperarPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecuperarPassword([Bind(Include = "email")] personas persona)
        {
            if (Session["user_id"] == null)
            {
                if (ModelState.IsValid)
                {
                    //creo la nueva contraseña
                    string parol = "";
                    string valores = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM01234567890123456789";
                    string body, email, cadena;
                    bool correcto;
                    SqlConnection conexion;
                    SqlCommand consulta1, consulta2;
                    SqlDataReader existe;
                    enviarCorreo correo;
                    personas p;

                    Random rnd = new Random();

                    for (int i = 0; i < 10; i++)
                        parol += valores[rnd.Next(0, valores.Length)];

                    email = persona.email;
                    cadena = System.Configuration.ConfigurationManager.ConnectionStrings["alternativa"].ConnectionString;
                    conexion = new SqlConnection(cadena);

                    conexion.Open();

                    consulta1 = new SqlCommand("SELECT id FROM personas WHERE upper(email) = '" + email.ToUpper() + "'", conexion);
                    //consulta1 = new SqlCommand(" update personas set password = '"+ parol +"' where email='" + email + "'", conexion);

                    try
                    {
                        existe = consulta1.ExecuteReader();

                        if (existe.Read())
                        {
                            conexion.Close();
                            conexion.Open();
                            consulta2 = new SqlCommand("UPDATE personas SET password = '" + parol + "' WHERE upper(email) = '" + email.ToUpper() + "'", conexion);
                            consulta2.ExecuteNonQuery();

                            //envio el correo
                            correo = new enviarCorreo();
                            body = "Tu nueva contraseña es: " + parol + "\n \n Recuerda cambiarla al iniciar sesión.";
                            correcto = correo.SendMail(persona.email, "Gato Mensajero recuperación contraseña", body);

                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ViewBag.CustomError = "El correo introducido no existe. Por favor, vuelve a intentarlo.";
                            return View("RecuperarPassword");
                        }

                    }
                    catch (Exception e)
                    {
                        ViewBag.CustomError = "Ocurrió un error. Contacte con el Administrador" + e;
                        return View("RecuperarPassword");
                    }
                    finally
                    {
                        conexion.Close();
                    }
                }
                return View();
            }
            else
                return RedirectToAction("Index");
                
        }

        public ActionResult LogOut()
        {
            if (Session["user_id"] != null)
            {
                Session["user_id"] = null;
                Session["user_email"] = null;
                Session["user_nombre"] = null;
                Session["user_apellidos"] = null;
                Session["user_rol"] = null;
            }

            return RedirectToAction("Index");
        }

        public void CreateSession(int id, string e, string n, string a, string r)
        {
            Session["user_id"] = id;
            Session["user_email"] = e;
            Session["user_nombre"] = n;
            Session["user_apellidos"] = a;
            Session["user_rol"] = r;
        }
    }
}
