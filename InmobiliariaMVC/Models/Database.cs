// Reemplaza TODO el contenido de este archivo

using MySql.Data.MySqlClient;

namespace InmobiliariaMVC.Models
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Este es el método clave que tus repositorios necesitan
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}