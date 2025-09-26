using InmobiliariaMVC.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioContrato
    {
        private readonly Database _db;
        public RepositorioContrato(Database db) => _db = db;

        public List<Contrato> ObtenerTodos()
        {
            var lista = new List<Contrato>();
            using var conn = _db.GetConnection();
            conn.Open();

            string sql = @"SELECT c.*, 
                      i.Direccion, 
                      inq.Nombre AS NombreInq, inq.Apellido AS ApellidoInq,
                      u.Nombre AS NombreCreador, u.Apellido AS ApellidoCreador,
                      u2.Nombre AS NombreAnulador, u2.Apellido AS ApellidoAnulador
               FROM Contratos c
               JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
               JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
               LEFT JOIN Usuarios u ON c.CreadoPor = u.IdUsuario
               LEFT JOIN Usuarios u2 ON c.AnuladoPor = u2.IdUsuario
               ORDER BY c.IdContrato DESC";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("IdContrato"),
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    IdInquilino = reader.GetInt32("IdInquilino"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    PrecioMensual = reader.GetDecimal("PrecioMensual"),
                    Estado = reader.GetBoolean("Estado"),
                    CreadoPor = reader.GetInt32("CreadoPor"),
                    FechaCreacion = reader.GetDateTime("FechaCreacion"),
                    AnuladoPor = reader.IsDBNull(reader.GetOrdinal("AnuladoPor")) ? null : reader.GetInt32("AnuladoPor"),
                    FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? null : reader.GetDateTime("FechaAnulacion"),
                    Inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32("IdInmueble"),
                        Direccion = reader.GetString("Direccion")
                    },
                    Inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32("IdInquilino"),
                        Nombre = reader.GetString("NombreInq"),
                        Apellido = reader.GetString("ApellidoInq")
                    },
                    UsuarioCreador = new Usuario
                    {
                        Nombre = reader.IsDBNull(reader.GetOrdinal("NombreCreador")) ? "" : reader.GetString("NombreCreador"),
                        Apellido = reader.IsDBNull(reader.GetOrdinal("ApellidoCreador")) ? "" : reader.GetString("ApellidoCreador")
                    },
                    UsuarioAnulador = new Usuario
                    {
                        Nombre = reader.IsDBNull(reader.GetOrdinal("NombreAnulador")) ? "" : reader.GetString("NombreAnulador"),
                        Apellido = reader.IsDBNull(reader.GetOrdinal("ApellidoAnulador")) ? "" : reader.GetString("ApellidoAnulador")
                    }
                });

            }
            return lista;
        }

        public List<Contrato> ObtenerPorEstado(bool estado)
        {
            var lista = new List<Contrato>();
            using var conn = _db.GetConnection();
            conn.Open();

            string sql = @"SELECT c.*, i.Direccion, inq.Nombre, inq.Apellido
                   FROM Contratos c
                   JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                   JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                   WHERE c.Estado = @estado
                   ORDER BY c.IdContrato DESC";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@estado", estado);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Contrato
                {
                    IdContrato = reader.GetInt32("IdContrato"),
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    IdInquilino = reader.GetInt32("IdInquilino"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    PrecioMensual = reader.GetDecimal("PrecioMensual"),
                    Estado = reader.GetBoolean("Estado"),
                    Inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32("IdInmueble"),
                        Direccion = reader.GetString("Direccion")
                    },
                    Inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32("IdInquilino"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido")
                    }
                });
            }
            return lista;
        }

        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = @"SELECT c.*, i.Direccion, inq.Nombre, inq.Apellido
                           FROM Contratos c
                           JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                           JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                           WHERE c.IdContrato = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                contrato = new Contrato
                {
                    IdContrato = reader.GetInt32("IdContrato"),
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    IdInquilino = reader.GetInt32("IdInquilino"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    PrecioMensual = reader.GetDecimal("PrecioMensual"),
                    Estado = reader.GetBoolean("Estado"),
                    Inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32("IdInmueble"),
                        Direccion = reader.GetString("Direccion")
                    },
                    Inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32("IdInquilino"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido")
                    }
                };
            }
            return contrato;
        }

        public void Alta(Contrato c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"
        INSERT INTO Contratos 
        (IdInmueble, IdInquilino, FechaInicio, FechaFin, PrecioMensual, Estado, CreadoPor, FechaCreacion) 
        VALUES 
        (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @PrecioMensual, @Estado, @CreadoPor, NOW())", conn);

            cmd.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
            cmd.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
            cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", c.FechaFin);
            cmd.Parameters.AddWithValue("@PrecioMensual", c.PrecioMensual);
            cmd.Parameters.AddWithValue("@Estado", c.Estado);
            cmd.Parameters.AddWithValue("@CreadoPor", c.CreadoPor);

            cmd.ExecuteNonQuery();
        }



        public void Modificacion(Contrato c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE Contratos SET 
                                        IdInmueble=@IdInmueble, 
                                        IdInquilino=@IdInquilino, 
                                        FechaInicio=@FechaInicio, 
                                        FechaFin=@FechaFin, 
                                        PrecioMensual=@PrecioMensual, 
                                        Estado=@Estado 
                                        WHERE IdContrato=@IdContrato", conn);
            cmd.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
            cmd.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
            cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", c.FechaFin);
            cmd.Parameters.AddWithValue("@PrecioMensual", c.PrecioMensual);
            cmd.Parameters.AddWithValue("@Estado", c.Estado);
            cmd.Parameters.AddWithValue("@IdContrato", c.IdContrato);
            cmd.ExecuteNonQuery();
        }

        public void Baja(int id, int anuladoPor)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"UPDATE Contratos 
                                 SET Estado = 0, AnuladoPor=@anuladoPor, FechaAnulacion=NOW() 
                                 WHERE IdContrato=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@anuladoPor", anuladoPor);
            cmd.ExecuteNonQuery();
        }

        public bool InmuebleDisponible(int idInmueble, DateTime fechaInicio, DateTime fechaFin)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var query = @"SELECT COUNT(*) 
                  FROM Contratos 
                  WHERE IdInmueble = @idInmueble
                  AND (
                        (FechaInicio <= @fechaFin AND FechaFin >= @fechaInicio)
                      )";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idInmueble", idInmueble);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0; // true si está libre
        }
        public IList<Contrato> ObtenerPorDniInquilino(string dni)
        {
            var lista = new List<Contrato>();
            using var conn = _db.GetConnection();
            conn.Open();

            string sql = @"SELECT c.IdContrato, c.IdInquilino, c.IdInmueble, c.FechaInicio, c.FechaFin,
                          c.PrecioMensual, c.Estado,
                          inq.Nombre, inq.Apellido, inq.DNI,
                          inm.Direccion
                   FROM Contratos c
                   JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino
                   JOIN Inmuebles inm ON c.IdInmueble = inm.IdInmueble
                   WHERE inq.DNI = @dni";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@dni", dni);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var contrato = new Contrato
                {
                    IdContrato = reader.GetInt32("IdContrato"),
                    IdInquilino = reader.GetInt32("IdInquilino"),
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    FechaInicio = reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.GetDateTime("FechaFin"),
                    PrecioMensual = reader.GetDecimal("PrecioMensual"),
                    Estado = reader.GetBoolean("Estado"),
                    Inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32("IdInquilino"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Dni = reader.GetString("DNI")
                    },
                    Inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32("IdInmueble"),
                        Direccion = reader.GetString("Direccion")
                    }
                };
                lista.Add(contrato);
            }
            return lista;
        }
        public bool InmuebleDisponible(int idInmueble, DateTime fechaInicio, DateTime fechaFin, int? idContrato = null)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            var query = @"SELECT COUNT(*) 
                  FROM Contratos 
                  WHERE IdInmueble = @idInmueble
                  AND (
                        (FechaInicio <= @fechaFin AND FechaFin >= @fechaInicio)
                      )
                  AND Estado = 1";

            if (idContrato.HasValue)
                query += " AND IdContrato <> @idContrato";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idInmueble", idInmueble);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
            if (idContrato.HasValue)
                cmd.Parameters.AddWithValue("@idContrato", idContrato.Value);

            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0; // true si está libre
        }
        public List<Inmueble> ObtenerInmueblesDisponibles(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<Inmueble>();
            using var conn = _db.GetConnection();
            conn.Open();

            string sql = @"
        SELECT i.* 
        FROM Inmuebles i
        WHERE i.IdInmueble NOT IN (
            SELECT c.IdInmueble
            FROM Contratos c
            WHERE c.Estado = 1
            AND (
                (c.FechaInicio <= @fechaFin AND c.FechaFin >= @fechaInicio)
            )
        )";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Inmueble
                {
                    IdInmueble = reader.GetInt32("IdInmueble"),
                    IdPropietario = reader.GetInt32("IdPropietario"),
                    Direccion = reader.GetString("Direccion"),
                    Uso = reader.IsDBNull(reader.GetOrdinal("Uso")) ? null : reader.GetString("Uso"),
                    Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? null : reader.GetString("Tipo"),
                    Ambientes = reader.IsDBNull(reader.GetOrdinal("Ambientes")) ? null : reader.GetInt32("Ambientes"),
                    Superficie = reader.IsDBNull(reader.GetOrdinal("Superficie")) ? null : reader.GetDecimal("Superficie"),
                    Precio = reader.GetDecimal("Precio"),
                    Disponible = reader.GetBoolean("Disponible"),
                });
            }

            return lista;
        }

    }

}
