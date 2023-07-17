using Ejercicio2_4.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Ejercicio2_4.Helpers
{
    public class SQLiteHelper
    {
        private SQLiteConnection _database;

        public SQLiteHelper()
        {
            // Ruta de la base de datos (ubicación del archivo SQLite en el dispositivo)
            var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "signatures.db3");

            // Crear la conexión a la base de datos
            _database = new SQLiteConnection(databasePath);

            // Crear la tabla si no existe
            _database.CreateTable<Signatures>();
        }

        public SQLiteConnection GetConnection()
        {
            return _database;
        }
    }

}
