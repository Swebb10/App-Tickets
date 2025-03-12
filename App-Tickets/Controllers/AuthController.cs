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

        // POST: /Auth/Registro y sksdjksjdksdjksd
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
                    cmd.Parameters.Add(new SqlParameter("@Contraseña", usuario.Password));
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
            Console.WriteLine($"Correo: {correo}");
            Console.WriteLine($"Contraseña: {contraseña}");

            Usuario usuario = null;

            // Iniciar conexión a la base de datos
            using (var context = new ApplicationDbContext())
            {
                // Crear el comando SQL para ejecutar el stored procedure
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = "EXEC sp_AutenticarUsuario @Email, @Contraseña";

                // Agregar los parámetros al comando
                cmd.Parameters.Add(new SqlParameter("@Email", correo));
                cmd.Parameters.Add(new SqlParameter("@Contraseña", contraseña)); // Asegúrate de que esta función hash está alineada con la base de datos

                // Abrir la conexión a la base de datos
                context.Database.Connection.Open();

                // Ejecutar el comando y leer los resultados
                var reader = cmd.ExecuteReader();

                if (reader.Read()) // Si encontramos un resultado
                {
                    usuario = new Usuario
                    {
                        Id = reader["ID_Usuario"].ToString(),
                        Correo = reader["Email"].ToString(),
                        Nombre = reader["Nombre"].ToString(),
                        Password = reader["Password"].ToString(),
                        PrimerApellido = reader["Primer_Apellido"].ToString(),
                        SegundoApellido = reader["Segundo_Apellido"].ToString(),
                        Rol = reader["Rol_Usuario"].ToString()
                    };
                }
                reader.Close();
                context.Database.Connection.Close();
            }

            // Si no encontramos al usuario o la contraseña no coincide
            if (usuario != null)
            {
                // Establecer cookie de autenticación
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                // Redirigir según el rol
                if (usuario.Rol == "Soporte")
                {
                    return RedirectToAction("Tickets", "Auth");
                }
                else
                {
                    return RedirectToAction("Tickets", "Auth");
                }
            }
            else
            {
                // Si el usuario no se encontró o la contraseña no coincide
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
        /*private string HashPassword(string password)
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
        }*/

        //Establecimiento para la visualizacion de los tickets, ventana
        public ActionResult Tickets()
        {
            return View();
        }

        //Segcion de guardado de los tickets
        [HttpPost]
        public ActionResult CreacionTickets(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                using (var context = new ApplicationDbContext())
                {
                    var cmd = context.Database.Connection.CreateCommand();
                    cmd.CommandText = "EXEC sp_CrearTicket @Asunto, @ID_Categoria, @ID_NivelUrgencia, @ID_NivelImportancia, @Creado_Por, @Asignado_A";

                    cmd.Parameters.Add(new SqlParameter("@Asunto", ticket.Asunto));
                    cmd.Parameters.Add(new SqlParameter("@ID_Categoria", ticket.CategoriaId));
                    cmd.Parameters.Add(new SqlParameter("@ID_NivelUrgencia", ticket.UrgenciaId));
                    cmd.Parameters.Add(new SqlParameter("@ID_NivelImportancia", ticket.Importancia));
                    cmd.Parameters.Add(new SqlParameter("@Creado_Por", ticket.UsuarioId));
                    cmd.Parameters.Add(new SqlParameter("@Asignado_A", ticket.UsuarioId));

                    context.Database.Connection.Open();
                    cmd.ExecuteNonQuery();
                    context.Database.Connection.Close();
                }

                return RedirectToAction("Login");
            }
            return View(ticket);
        }

        public ActionResult DashboardAnalista()
        {
            return View();
        }

        public ActionResult VistaAnalista()
        {
            return View();
        }
        public ActionResult VistaSoporte()
        {
            return View();
        }

    } 
}