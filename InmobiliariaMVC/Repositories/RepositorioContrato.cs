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

            string sql = @"SELECT c.*, i.Direccion, inq.Nombre, inq.Apellido
                           FROM Contratos c
                           JOIN Inmuebles i ON c.IdInmueble = i.IdInmueble
                           JOIN Inquilinos inq ON c.IdInquilino = inq.IdInquilino";

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
            var cmd = new MySqlCommand(@"INSERT INTO Contratos 
                                         (IdInmueble, IdInquilino, FechaInicio, FechaFin, PrecioMensual, Estado) 
                                         VALUES 
                                         (@IdInmueble, @IdInquilino, @FechaInicio, @FechaFin, @PrecioMensual, @Estado)", conn);
            cmd.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
            cmd.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
            cmd.Parameters.AddWithValue("@FechaInicio", c.FechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", c.FechaFin);
            cmd.Parameters.AddWithValue("@PrecioMensual", c.PrecioMensual);
            cmd.Parameters.AddWithValue("@Estado", c.Estado);
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

        public void Baja(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Contratos WHERE IdContrato=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
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

    }
}
