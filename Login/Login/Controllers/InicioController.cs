using Login.Models;
using Login.Recursos;
using Login.Servicios.Contratos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Login.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public InicioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Registrarse()
        {
            return View();
        }

        //Responde la solicitud del método anterior
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);

            Usuario usuarioCreado = await _usuarioService.SaveUsuario(modelo);

            if (usuarioCreado.IdUsuario > 0)
                return RedirectToAction("IniciarSesion", "Inicio"); //"IniciarSesion" es el nombre del metodo e "Inicio" el nombre del controller que almacena el metodo

            ViewData["Mensaje"] = "No fue posible crear el usuario";

            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        //Responde la solicitud del método anterior
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            Usuario usuarioEncontrado = await _usuarioService.GetUsuario(correo);

            if (usuarioEncontrado == null)
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }
            else if (usuarioEncontrado.Clave != Utilidades.EncriptarClave(clave))
            {
                ViewData["Mensaje"] = "Contraseña incorrecta";
                return View();
            }

            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );
            return RedirectToAction("Index", "Home"); //"Index" es el nombre del metodo e "Home" el nombre del controller que almacena el metodo
        }
    }
}
