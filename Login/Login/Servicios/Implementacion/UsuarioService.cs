using Microsoft.EntityFrameworkCore;
using Login.Models;
using Login.Servicios.Contratos;

namespace Login.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DbloginContext _connection;

        public UsuarioService(DbloginContext connection)
        {
            _connection = connection;
        }

        public async Task<Usuario> GetUsuario(string correo, string clave)
        {
            Usuario usuarioEncontrado = await _connection.Usuarios.Where(usuario => usuario.Correo == correo && usuario.Clave == clave)
                .FirstOrDefaultAsync();

            return usuarioEncontrado;
        }

        public async Task<Usuario> SaveUsuario(Usuario modelo)
        {
            _connection.Usuarios.Add(modelo);
            await _connection.SaveChangesAsync();
            return modelo;
        }
    }
}
