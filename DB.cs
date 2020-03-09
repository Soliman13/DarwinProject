using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    class DB
    {
        private const string DB_PATH = "../db.db3";
        private static DB db;
        public static DB GetDataBase()
        {
            if (db == null)
            {
                db = new DB();
            }
            return db;
        }
        private DB() { }

        public void CreateTableParams()
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.CreateTable<Params>();
        }

        public void CreateTableCity()
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.CreateTable<City>();
        }

        public void InsertCity(City c)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.CreateTable<City>();
            connection.Insert(c);
        }
        
        public List<City> SelectCity(string query)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            List<City> cities = connection.Query<City>(query);
            foreach (City c in cities)
            {
                Console.WriteLine("City : " + c.Id + " ; " + c.Name + " ; " + c.X + " ; " + c.Y);
            }
            return cities;
        }

        public void InsertParams(Params p)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.CreateTable<Params>();
            connection.InsertOrReplace(p);
        }
        
        public List<Params> SelectParams(string query)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            List<Params> listParams = connection.Query<Params>(query);
            foreach (Params p in listParams)
            {
                Console.WriteLine("Param : " + p.Key + " ; " + p.Value);
            }
            return listParams;
        }

        public void DeleteCity(City c)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.Delete<City>(c.Id);
        }

        public void DeleteParams(Params p)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.Delete(p);
        }

        public void UpdateParams(Params p)
        {
            SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(DB_PATH);
            connection.InsertOrReplace(p);
        }
    }
}
