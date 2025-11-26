using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Database;
using Dapper;     
using System.Data.SQLite;

namespace TextEditor.Features.Bookmarks
{
    public class SqlBookmarkRepository : IBookmarkRepository
    {
        private readonly DatabaseService _db;

        public SqlBookmarkRepository(DatabaseService db)
        {
            _db = db;
        }

        public void Add(Bookmark bookmark)
        {
            using (var con = _db.GetConnection())
            {
                con.Execute("INSERT INTO Bookmarks (FilePath, Line, CreatedAt) VALUES (@FilePath, @Line, @CreatedAt)", bookmark);
            }
        }

        public void Remove(string filePath, int line)
        {
            using (var con = _db.GetConnection())
            {
                con.Execute("DELETE FROM Bookmarks WHERE FilePath = @FilePath AND Line = @Line", new { FilePath = filePath, Line = line });
            }
        }

        public List<Bookmark> GetByFile(string filePath)
        {
            using (var con = _db.GetConnection())
            {
                return con.Query<Bookmark>("SELECT * FROM Bookmarks WHERE FilePath = @FilePath ORDER BY Line", new { FilePath = filePath }).AsList();
            }
        }

        public bool Exists(string filePath, int line)
        {
            using (var con = _db.GetConnection())
            {
                int count = con.QuerySingle<int>("SELECT COUNT(*) FROM Bookmarks WHERE FilePath = @FilePath AND Line = @Line", new { FilePath = filePath, Line = line });
                return count > 0;
            }
        }
    }
}
