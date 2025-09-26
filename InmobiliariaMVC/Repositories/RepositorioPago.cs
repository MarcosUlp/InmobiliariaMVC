using InmobiliariaMVC.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioPago
    {
        private readonly Database _db;
        public RepositorioPago(Database db) => _db = db;

        public List<Pago> ObtenerTodos(bool activos = true)
        {
            var lista = new List<Pago>();
            using var conn = _db.GetConnection();
            conn.Open();
            var sql = @"
    SELECT p.IdPago, p.IdContrato, p.FechaPago, p.Monto, p.Observaciones,
           p.Estado, p.CreadoPor, p.FechaCreacion, p.AnuladoPor, p.FechaAnulacion,
           c.IdContrato AS ContratoId,
           i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
           m.Direccion AS InmuebleDireccion
    FROM Pagos p
    INNER JOIN Contratos c ON p.IdContrato = c.IdContrato
    INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino
    INNER JOIN Inmuebles m ON c.IdInmueble = m.IdInmueble
    ORDER BY p.FechaPago DESC;"; // 👈 saco el WHERE p.Estado=@estado
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@estado", activos ? 1 : 0);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add(new Pago
                {
                    IdPago = reader.GetInt32("IdPago"),
                    IdContrato = reader.GetInt32("IdContrato"),
                    FechaPago = reader.GetDateTime("FechaPago"),
                    Monto = reader.GetDecimal("Monto"),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? "" : reader.GetString("Observaciones"),
                    Estado = reader.GetInt32("Estado") == 1,
                    CreadoPor = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? null : reader.GetInt32("CreadoPor"),
                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("AnuladoPor")) ? null : reader.GetInt32("AnuladoPor"),
                    FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? null : reader.GetDateTime("FechaAnulacion"),
                    Contrato = new Contrato
                    {
                        IdContrato = reader.GetInt32("ContratoId"),
                        Inquilino = new Inquilino
                        {
                            Nombre = reader.GetString("InquilinoNombre"),
                            Apellido = reader.GetString("InquilinoApellido")
                        },
                        Inmueble = new Inmueble
                        {
                            Direccion = reader.GetString("InmuebleDireccion")
                        }
                    }
                });
            return lista;
        }
        public Usuario ObtenerUsuario(int id)
        {
            Usuario usuario = null!;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT IdUsuario, Nombre, Apellido FROM Usuarios WHERE IdUsuario=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                usuario = new Usuario
                {
                    IdUsuario = reader.GetInt32("IdUsuario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido")
                };
            }
            return usuario;
        }

        public Pago? ObtenerPorId(int id)
        {
            Pago? pago = null;
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Pagos WHERE IdPago=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                pago = new Pago
                {
                    IdPago = reader.GetInt32("IdPago"),
                    IdContrato = reader.GetInt32("IdContrato"),
                    FechaPago = reader.GetDateTime("FechaPago"),
                    Monto = reader.GetDecimal("Monto"),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? "" : reader.GetString("Observaciones")
                };
            }
            return pago;
        }

        public void Alta(Pago p, int usuarioId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
        INSERT INTO Pagos 
            (IdContrato, FechaPago, Monto, Observaciones, CreadoPor, FechaCreacion, Estado) 
        VALUES 
            (@idContrato, @fecha, @monto, @obs, @creadoPor, @fechaCreacion, 1)", conn);

            cmd.Parameters.AddWithValue("@idContrato", p.IdContrato);
            cmd.Parameters.AddWithValue("@fecha", p.FechaPago);
            cmd.Parameters.AddWithValue("@monto", p.Monto);
            cmd.Parameters.AddWithValue("@obs", string.IsNullOrEmpty(p.Observaciones) ? (object)DBNull.Value : p.Observaciones);
            cmd.Parameters.AddWithValue("@creadoPor", usuarioId);
            cmd.Parameters.AddWithValue("@fechaCreacion", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void Editar(Pago p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var query = @"UPDATE Pagos 
                          SET IdContrato=@idContrato, FechaPago=@fecha, Monto=@monto, Observaciones=@obs 
                          WHERE IdPago=@id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idContrato", p.IdContrato);
            cmd.Parameters.AddWithValue("@fecha", p.FechaPago);
            cmd.Parameters.AddWithValue("@monto", p.Monto);
            cmd.Parameters.AddWithValue("@obs", string.IsNullOrEmpty(p.Observaciones) ? (object)DBNull.Value : p.Observaciones);
            cmd.Parameters.AddWithValue("@id", p.IdPago);
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id, int usuarioId)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
        UPDATE Pagos 
        SET Estado = 0, AnuladoPor=@anuladoPor, FechaAnulacion=@fechaAnulacion
        WHERE IdPago = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@anuladoPor", usuarioId);
            cmd.Parameters.AddWithValue("@fechaAnulacion", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

    }
}
