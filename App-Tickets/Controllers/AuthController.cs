using App_Tickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace App_Tickets.Controllers
{
    public class AuthController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Registro()
        {
            return View();
        }

        // POST: /Auth/Registro
        [HttpPost]
        public ActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo ya existe
                if (db.Usuarios.Any(u => u.Correo == usuario.Correo))
                {
                    ModelState.AddModelError("Correo", "El correo ya está registrado.");
                    return View(usuario);
                }

                // Encriptar la contraseña antes de guardarla con SHA256
                usuario.Contraseña = HashPassword(usuario.Contraseña);

                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(usuario);
        }

        // GET: /Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        public ActionResult Login(string correo, string contraseña)
        {
            string passwordEncriptada = HashPassword(contraseña);

            var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contraseña == passwordEncriptada);
            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Rol)) // Verifica que tenga un rol asignado
                {
                    ViewBag.Mensaje = "Error: No tienes un rol asignado.";
                    return View();
                }

                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                // Redirigir según el rol del usuario
                if (usuario.Rol == "Soporte")
                {
                    return RedirectToAction("DashboardSoporte", "Home");
                }
                else if (usuario.Rol == "Analista")
                {
                    return RedirectToAction("DashboardAnalista", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Mensaje = "Correo o contraseña incorrectos.";
                return View();
            }
        }



        // Cerrar sesión
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // Método para encriptar contraseñas con SHA256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}