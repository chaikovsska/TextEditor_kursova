using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace TextEditor.Database
{
    public class DatabaseService
    {
        private readonly string _dbPath = "Database/editor.db";
        private readonly string _connectionString;

        public DatabaseService()
        {
            string folder = Path.GetDirectoryName(_dbPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!File.Exists(_dbPath))
                SQLiteConnection.CreateFile(_dbPath);

            _connectionString = $"Data Source={_dbPath};Version=3;";

            CreateTables();

            SeedSnippets();
        }

        private void CreateTables()
        {
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();

                con.Execute(@"
                    CREATE TABLE IF NOT EXISTS Bookmarks (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FilePath TEXT,
                        Line INTEGER,
                        CreatedAt TEXT
                    );
                ");

                con.Execute(@"
                    CREATE TABLE IF NOT EXISTS Snippets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT,
                        Shortcut TEXT UNIQUE, 
                        Content TEXT
                    );
                ");

                con.Execute(@"
                    CREATE TABLE IF NOT EXISTS Macros (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Actions TEXT,
                        Hotkey TEXT
                    );
                ");
            }
        }

        private void SeedSnippets()
        {
            using (var con = new SQLiteConnection(_connectionString))
            {
                con.Open();

                int count = con.QuerySingle<int>("SELECT COUNT(*) FROM Snippets");

                if (count == 0)
                {
                    var defaults = new List<dynamic>
                    {
                        new { Title = "Console Write", Shortcut = "cw", Content = "Console.WriteLine();" },
                        new { Title = "For Loop", Shortcut = "for", Content = "for (int i = 0; i < length; i++)\n{\n\t\n}" },
                        new { Title = "Public Class", Shortcut = "class", Content = "public class MyClass\n{\n\t\n}" },
                        new { Title = "Main Method", Shortcut = "svm", Content = "static void Main(string[] args)\n{\n\t\n}" },
                        new { Title = "If Statement", Shortcut = "if", Content = "if (true)\n{\n\t\n}" }
                    };

                    con.Execute("INSERT INTO Snippets (Title, Shortcut, Content) VALUES (@Title, @Shortcut, @Content)", defaults);
                }
            }
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}