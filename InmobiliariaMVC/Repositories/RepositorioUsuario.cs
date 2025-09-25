using InmobiliariaMVC.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioUsuario
    {
        private readonly Database _db;
        public RepositorioUsuario(Database db) => _db = db;

        public int Alta(Usuario usuario)
        {
            using var conn = _db.GetConnection();
            var sql = @"INSERT INTO Usuarios (Nombre, Apellido, Email, ClaveHash, Rol, Avatar) 
                        VALUES (@Nombre, @Apellido, @Email, @ClaveHash, @Rol, @Avatar);
                        SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@ClaveHash", usuario.ClaveHash);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            conn.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Modificar(Usuario usuario)
        {
            using var conn = _db.GetConnection();
            var sql = @"UPDATE Usuarios 
                        SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email, ClaveHash=@ClaveHash, Rol=@Rol, Avatar=@Avatar 
                        WHERE IdUsuario=@IdUsuario";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@ClaveHash", usuario.ClaveHash);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var conn = _db.GetConnection();
            var sql = @"DELETE FROM Usuarios WHERE IdUsuario=@IdUsuario";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@IdUsuario", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public Usuario? ObtenerPorId(int id)
        {
            Usuario? usuario = null;
            using var conn = _db.GetConnection();
            var sql = @"SELECT * FROM Usuarios WHERE IdUsuario=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario
                {
                    IdUsuario = reader.GetInt32("IdUsuario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Email = reader.GetString("Email"),
                    ClaveHash = reader.GetString("ClaveHash"),
                    Rol = reader.GetString("Rol"),
                    Avatar = reader["Avatar"] as string
                };
            }
            return usuario;
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            Usuario? usuario = null;
            using var conn = _db.GetConnection();
            var sql = @"SELECT * FROM Usuarios WHERE Email=@Email";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario
                {
                    IdUsuario = reader.GetInt32("IdUsuario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Email = reader.GetString("Email"),
                    ClaveHash = reader.GetString("ClaveHash"),
                    Rol = reader.GetString("Rol"),
                    Avatar = reader["Avatar"] as string
                };
            }
            return usuario;
        }

        public List<Usuario> ObtenerTodos()
        {
            var lista = new List<Usuario>();
            using var conn = _db.GetConnection();
            var sql = "SELECT * FROM Usuarios";
            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32("IdUsuario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Email = reader.GetString("Email"),
                    ClaveHash = reader.GetString("ClaveHash"),
                    Rol = reader.GetString("Rol"),
                    Avatar = reader["Avatar"] as string
                });
            }
            return lista;
        }
    }
}
