using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Database;

namespace TextEditor.Features.Snippets
{
    public class SqlSnippetRepository : ISnippetRepository
    {
        private readonly DatabaseService _db;

        public SqlSnippetRepository(DatabaseService db)
        {
            _db = db;
        }

        public void Add(Snippet snippet)
        {
            using (var con = _db.GetConnection())
            {
                con.Execute(
                    "INSERT INTO Snippets (Title, Shortcut, Content) VALUES (@Title, @Shortcut, @Content)",
                    snippet);
            }
        }

        public List<Snippet> GetAll()
        {
            using (var con = _db.GetConnection())
            {
                return con.Query<Snippet>("SELECT * FROM Snippets").AsList();
            }
        }
    }
}
