using InmobiliariaMVC.Models;
using MySql.Data.MySqlClient;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioInmueble
    {
        private readonly Database _db;
        public RepositorioInmueble(Database db) => _db = db;

        public List<Inmueble> ObtenerTodos()
        {
            var lista = new List<Inmueble>();
            using var connection = _db.GetConnection();
            connection.Open();
            string sql = @"SELECT i.IdInmueble, i.Direccion, i.Uso, i.Tipo, i.Ambientes, i.Superficie, i.Precio, i.Disponible,
                                      p.IdPropietario, p.Nombre, p.Apellido
                               FROM Inmuebles i
                               JOIN Propietarios p ON i.IdPropietario = p.IdPropietario";
            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    Direccion = reader.GetString("Direccion"),
                    Uso = reader.IsDBNull(reader.GetOrdinal("Uso")) ? null : reader.GetString("Uso"),
                    Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? null : reader.GetString("Tipo"),
                    Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? (int?)null : reader.GetInt32("Ambientes"),
                    Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? (decimal?)null : reader.GetDecimal("Superficie"),
                    Precio = reader.GetDecimal("Precio"),
                    Disponible = reader.GetBoolean("Disponible"),
                    IdPropietario = reader.GetInt32("IdPropietario"),
                    Dueño = new Propietario
                    {
                        IdPropietario = reader.GetInt32("IdPropietario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido")
                    }
                });
            }
            return lista;
        }
        
        public Inmueble? ObtenerPorId(int id)
        {
             Inmueble? inmueble = null;
             using var connection = _db.GetConnection();
             connection.Open();
             string sql = @"SELECT i.*, p.Nombre, p.Apellido 
                            FROM Inmuebles i JOIN Propietarios p ON i.IdPropietario = p.IdPropietario 
                            WHERE i.IdInmueble = @id";
             using var command = new MySqlCommand(sql, connection);
             command.Parameters.AddWithValue("@id", id);
             using var reader = command.ExecuteReader();
             if (reader.Read())
             {
                 inmueble = new Inmueble
                 {
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    Direccion = reader.GetString("Direccion"),
                    Uso = reader.IsDBNull(reader.GetOrdinal("Uso")) ? null : reader.GetString("Uso"),
                    Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? null : reader.GetString("Tipo"),
                    Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? (int?)null : reader.GetInt32("Ambientes"),
                    Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? (decimal?)null : reader.GetDecimal("Superficie"),
                    Precio = reader.GetDecimal("Precio"),
                    Disponible = reader.GetBoolean("Disponible"),
                    IdPropietario = reader.GetInt32("IdPropietario"),
                    Dueño = new Propietario
                    {
                        IdPropietario = reader.GetInt32("IdPropietario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido")
                    }
                 };
             }
             return inmueble;
        }

        public void Alta(Inmueble i)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO Inmuebles (IdPropietario, Direccion, Uso, Tipo, Ambientes, Superficie, Precio, Disponible) 
                                       VALUES (@IdPropietario, @Direccion, @Uso, @Tipo, @Ambientes, @Superficie, @Precio, @Disponible)", conn);
            cmd.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
            cmd.Parameters.AddWithValue("@Direccion", i.Direccion);
            cmd.Parameters.AddWithValue("@Uso", i.Uso);
            cmd.Parameters.AddWithValue("@Tipo", i.Tipo);
            cmd.Parameters.AddWithValue("@Ambientes", i.Ambientes);
            cmd.Parameters.AddWithValue("@Superficie", i.Superficie);
            cmd.Parameters.AddWithValue("@Precio", i.Precio);
            cmd.Parameters.AddWithValue("@Disponible", i.Disponible);
            cmd.ExecuteNonQuery();
        }

        public void Modificacion(Inmueble i)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE Inmuebles SET IdPropietario=@IdPropietario, Direccion=@Direccion, Uso=@Uso, Tipo=@Tipo, Ambientes=@Ambientes, Superficie=@Superficie, Precio=@Precio, Disponible=@Disponible
                                       WHERE IdInmueble = @IdInmueble", conn);
            cmd.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
            cmd.Parameters.AddWithValue("@Direccion", i.Direccion);
            cmd.Parameters.AddWithValue("@Uso", i.Uso);
            cmd.Parameters.AddWithValue("@Tipo", i.Tipo);
            cmd.Parameters.AddWithValue("@Ambientes", i.Ambientes);
            cmd.Parameters.AddWithValue("@Superficie", i.Superficie);
            cmd.Parameters.AddWithValue("@Precio", i.Precio);
            cmd.Parameters.AddWithValue("@Disponible", i.Disponible);
            cmd.Parameters.AddWithValue("@IdInmueble", i.IdInmueble);
            cmd.ExecuteNonQuery();
        }

        public void Baja(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Inmuebles WHERE IdInmueble=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}