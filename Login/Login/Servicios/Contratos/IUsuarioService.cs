using Microsoft.EntityFrameworkCore;
using Login.Models;


namespace Login.Servicios.Contratos
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> SaveUsuario(Usuario modelo);
    }
}
