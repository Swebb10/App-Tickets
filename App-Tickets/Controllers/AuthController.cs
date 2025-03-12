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
        [HttpPost]
        public ActionResult Login(string correo, string contraseña)
        {
            Usuario usuario = null;

            using (var context = new ApplicationDbContext())  // Manejo seguro de la conexión
            {
                context.Database.Connection.Open();  // Abrimos conexión

                using (var cmd = context.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = "EXEC sp_AutenticarUsuario @Email, @Contraseña";
                    cmd.Parameters.Add(new SqlParameter("@Email", correo));
                    cmd.Parameters.Add(new SqlParameter("@Contraseña", contraseña));

                    using (var reader = cmd.ExecuteReader())  // Manejo seguro del reader
                    {
                        if (reader.Read()) // Si encuentra un usuario
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
                    }
                }

                context.Database.Connection.Close();  // Cerramos la conexión
            }

            if (usuario != null)
            {
                FormsAuthentication.SetAuthCookie(usuario.Correo, false);
                return RedirectToAction("Tickets", "Auth");
            }
            else
            {
                ViewBag.Mensaje = "Correo o contraseña incorrectos.";
                return View();
            }
        }

        /*/ POST: /Auth/Login
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
        }*/

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


            /*
            private ApplicationDbContext db = new ApplicationDbContext();

            // GET: /Auth/Login
            [AllowAnonymous]
            public ActionResult Login()
            {
                return View();
            }

            // POST: /Auth/Login
            [HttpPost]
            [AllowAnonymous]
            public ActionResult Login(string correo, string contraseña)
            {
                try
                {
                    // Validar que el correo y la contraseña no estén vacíos
                    if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
                    {
                        ViewBag.Mensaje = "El correo y la contraseña son requeridos.";
                        return View();
                    }

                    Usuario usuario = null;

                    // Conectar a la base de datos y ejecutar el procedimiento almacenado
                    using (var context = new ApplicationDbContext())
                    {
                        var cmd = context.Database.Connection.CreateCommand();
                        cmd.CommandText = "EXEC sp_AutenticarUsuario @Email, @Contraseña";
                        cmd.Parameters.Add(new SqlParameter("@Email", correo));
                        cmd.Parameters.Add(new SqlParameter("@Contraseña", SecurityHelper.HashPassword(contraseña)));

                        context.Database.Connection.Open();
                        var reader = cmd.ExecuteReader();

                        // Leer los datos del usuario si existe
                        if (reader.Read())
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
                        reader.Close(); // Cerrar el reader explícitamente
                    }

                    // Verificar si el usuario existe
                    if (usuario != null)
                    {
                        // Crear una cookie de autenticación
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
                            // Redirigir a una vista por defecto si el rol no está definido
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        // Mostrar un mensaje de error si las credenciales son incorrectas
                        ViewBag.Mensaje = "Correo o contraseña incorrectos.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    // Logear el error (puedes usar un logger como NLog o Serilog)
                    ViewBag.Mensaje = "Ocurrió un error durante el inicio de sesión. Por favor, inténtalo de nuevo.";
                    return View();
                }
            }

            // GET: /Auth/Registro
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
                    // Hashear la contraseña antes de guardarla
                    usuario.Password = SecurityHelper.HashPassword(usuario.Password);

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

            // Cerrar sesión
            public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            */
        }
    }
}