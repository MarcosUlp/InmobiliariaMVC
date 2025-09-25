using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using InmobiliariaMVC.Models;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioPropietario
    {
        private readonly Database _db;
        public RepositorioPropietario(Database db) => _db = db;

        public List<Propietario> ObtenerTodos()
        {
            var lista = new List<Propietario>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Propietarios WHERE Estado=1", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Propietario
                {
                    IdPropietario = reader.GetInt32("IdPropietario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Dni = reader.GetString("Dni"),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString("Telefono"),
                    Email = reader.GetString("Email"),
                    Estado = reader.GetBoolean("Estado")
                });
            }
            return lista;
        }

        public Propietario? ObtenerPorId(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Propietarios WHERE IdPropietario=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Propietario
                {
                    IdPropietario = reader.GetInt32("IdPropietario"),
                    Nombre = reader.GetString("Nombre"),
                    Apellido = reader.GetString("Apellido"),
                    Dni = reader.GetString("Dni"),
                    Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? "" : reader.GetString("Telefono"),
                    Email = reader.GetString("Email"),
                    Estado = reader.GetBoolean("Estado")
                };
            }
            return null;
        }

        public int Alta(Propietario p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"INSERT INTO Propietarios (Nombre, Apellido, Dni, Telefono, Email, Estado) 
                  VALUES (@nombre,@apellido,@dni,@telefono,@email,1);
                  SELECT LAST_INSERT_ID();", conn);

            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@dni", p.Dni);
            cmd.Parameters.AddWithValue("@telefono", p.Telefono);
            cmd.Parameters.AddWithValue("@email", p.Email);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Editar(Propietario p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"UPDATE Propietarios 
                  SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
                  WHERE IdPropietario=@id", conn);

            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@dni", p.Dni);
            cmd.Parameters.AddWithValue("@telefono", p.Telefono);
            cmd.Parameters.AddWithValue("@email", p.Email);
            cmd.Parameters.AddWithValue("@id", p.IdPropietario);

            cmd.ExecuteNonQuery();
        }
        public bool TieneInmueblesConContratosActivos(int idPropietario)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"SELECT COUNT(*) 
          FROM Inmuebles i
          JOIN Contratos c ON i.IdInmueble = c.IdInmueble
          WHERE i.IdPropietario=@id AND c.Estado=1", conn);
            cmd.Parameters.AddWithValue("@id", idPropietario);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }

        public void Eliminar(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var tx = conn.BeginTransaction();

            try
            {
                // 1. Dar de baja al propietario
                var cmd = new MySqlCommand(
                    "UPDATE Propietarios SET Estado=0 WHERE IdPropietario=@id", conn, tx);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                // 2. Dar de baja inmuebles sin contratos activos
                var cmd2 = new MySqlCommand(
                    @"UPDATE Inmuebles i
              SET i.Disponible=0
              WHERE i.IdPropietario=@id 
              AND NOT EXISTS (
                  SELECT 1 FROM Contratos c 
                  WHERE c.IdInmueble = i.IdInmueble AND c.Estado=1
              )", conn, tx);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

    }
}
