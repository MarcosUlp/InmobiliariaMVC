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
            var sql = @"
        SELECT p.IdPago, p.IdContrato, p.FechaPago, p.Monto, p.Observaciones,
               c.IdContrato AS ContratoId,
               i.Nombre AS InquilinoNombre, i.Apellido AS InquilinoApellido,
               m.Direccion AS InmuebleDireccion
        FROM Pagos p
        INNER JOIN Contratos c ON p.IdContrato = c.IdContrato
        INNER JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino
        INNER JOIN Inmuebles m ON c.IdInmueble = m.IdInmueble
        ORDER BY p.FechaPago DESC;";   // 👈 ya ordenado DESC
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Pago
                {
                    IdPago = reader.GetInt32("IdPago"),
                    IdContrato = reader.GetInt32("IdContrato"),
                    FechaPago = reader.GetDateTime("FechaPago"),
                    Monto = reader.GetDecimal("Monto"),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? "" : reader.GetString("Observaciones"),
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
