using InmobiliariaMVC.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace InmobiliariaMVC.Repositories
{
    public class RepositorioPago
    {
        private readonly Database _db;
        public RepositorioPago(Database db) => _db = db;

        public List<Pago> ObtenerTodos()
        {
            var lista = new List<Pago>();
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Pagos", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Pago
                {
                    IdPago = reader.GetInt32("IdPago"),
                    IdContrato = reader.GetInt32("IdContrato"),
                    FechaPago = reader.GetDateTime("FechaPago"),
                    Monto = reader.GetDecimal("Monto"),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? "" : reader.GetString("Observaciones")
                });
            }
            return lista;
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

        public void Alta(Pago p)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand(@"INSERT INTO Pagos 
                (IdContrato, FechaPago, Monto, Observaciones) 
                VALUES (@idContrato, @fecha, @monto, @obs)", conn);
            cmd.Parameters.AddWithValue("@idContrato", p.IdContrato);
            cmd.Parameters.AddWithValue("@fecha", p.FechaPago);
            cmd.Parameters.AddWithValue("@monto", p.Monto);
            cmd.Parameters.AddWithValue("@obs", string.IsNullOrEmpty(p.Observaciones) ? (object)DBNull.Value : p.Observaciones);
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

        public void Eliminar(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Pagos WHERE IdPago=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
