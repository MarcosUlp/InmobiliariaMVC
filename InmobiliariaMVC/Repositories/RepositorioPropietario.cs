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
            using var cmd = new MySqlCommand("SELECT * FROM Propietarios", conn);
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
                    Clave = reader.GetString("Clave")
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
                    Clave = reader.GetString("Clave")
                };
            }
            return null; // si no existe
        }

        public int Alta(Propietario p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"INSERT INTO Propietarios (Nombre, Apellido, Dni, Telefono, Email, Clave) 
                  VALUES (@nombre,@apellido,@dni,@telefono,@email,@clave);
                  SELECT LAST_INSERT_ID();", conn);

            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@dni", p.Dni);
            cmd.Parameters.AddWithValue("@telefono", p.Telefono);
            cmd.Parameters.AddWithValue("@email", p.Email);
            cmd.Parameters.AddWithValue("@clave", p.Clave);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Editar(Propietario p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                @"UPDATE Propietarios 
                  SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email, Clave=@clave 
                  WHERE IdPropietario=@id", conn);

            cmd.Parameters.AddWithValue("@nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@dni", p.Dni);
            cmd.Parameters.AddWithValue("@telefono", p.Telefono);
            cmd.Parameters.AddWithValue("@email", p.Email);
            cmd.Parameters.AddWithValue("@clave", p.Clave);
            cmd.Parameters.AddWithValue("@id", p.IdPropietario);

            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("DELETE FROM Propietarios WHERE IdPropietario=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
