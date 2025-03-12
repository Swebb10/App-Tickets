using App_Tickets.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        [AllowAnonymous]
        public ActionResult Registro()
        {
            return View();
        }

        // POST: /Auth/Registro
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    var cmd = context.Database.Connection.CreateCommand();
                    cmd.CommandText = "EXEC sp_RegistrarUsuario @ID_Usuario, @Email, @Nombre, @Primer_Apellido, @Segundo_Apellido, @Contraseña, @Rol_Usuario";

                    cmd.Parameters.Add(new SqlParameter("@ID_Usuario", usuario.Id));
                    cmd.Parameters.Add(new SqlParameter("@Email", usuario.Correo));
                    cmd.Parameters.Add(new SqlParameter("@Nombre", usuario.Nombre));
                    cmd.Parameters.Add(new SqlParameter("@Primer_Apellido", usuario.PrimerApellido));
                    cmd.Parameters.Add(new SqlParameter("@Segundo_Apellido", usuario.SegundoApellido));
                    cmd.Parameters.Add(new SqlParameter("@Contraseña", HashPassword(usuario.Password)));
                    cmd.Parameters.Add(new SqlParameter("@Rol_Usuario", usuario.Rol));

                    context.Database.Connection.Open();
                    cmd.ExecuteNonQuery();
                    context.Database.Connection.Close();
                }

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
            Usuario usuario = null;
            using (var context = new ApplicationDbContext())
            {
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "EXEC sp_AutenticarUsuario @Email, @Contraseña";

                cmd.Parameters.Add(new SqlParameter("@Email", correo));
                cmd.Parameters.Add(new SqlParameter("@Contraseña", HashPassword(contraseña)));

                context.Database.Connection.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id = reader["ID_Usuario"].ToString(),
                        Correo = reader["Email"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        PrimerApellido = reader["Primer_Apellido"].ToString(),
                        SegundoApellido = reader["Segundo_Apellido"].ToString(),
                        Rol = reader["Rol_Usuario"].ToString()
                    };
                }
                reader.Close();
                context.Database.Connection.Close();
            }

            if (usuario != null)
            {
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);
                return usuario.Rol == "Soporte" ? RedirectToAction("DashboardSoporte", "Home") : RedirectToAction("DashboardAnalista", "Home");
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