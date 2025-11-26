using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Database;
using Dapper;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace TextEditor.Features.Macro
{
    public class SqlMacroRepository : IMacroRepository
    {
        private readonly DatabaseService _db;

        public SqlMacroRepository(DatabaseService db)
        {
            _db = db;
        }

        public void Save(Macro macro)
        {
            using (var con = _db.GetConnection())
            {
                con.Open();

                string jsonActions = JsonConvert.SerializeObject(macro.Actions);
                string jsonHotkey = JsonConvert.SerializeObject(macro.Hotkey);

                con.Execute(@"
                    INSERT INTO Macros (Name, Actions, Hotkey)
                    VALUES (@Name, @Actions, @Hotkey)",
                    new { macro.Name, Actions = jsonActions, Hotkey = jsonHotkey });
            }
        }

        public List<Macro> GetAll()
        {
            using (var con = _db.GetConnection())
            {
                con.Open();

                var rows = con.Query<dynamic>("SELECT * FROM Macros");

                List<Macro> list = new List<Macro>();

                foreach (var row in rows)
                {
                    list.Add(new Macro
                    {
                        Id = (int)row.Id,
                        Name = row.Name,
                        Actions = JsonConvert.DeserializeObject<List<MacroAction>>(row.Actions),
                        Hotkey = JsonConvert.DeserializeObject<Hotkey>(row.Hotkey)
                    });
                }

                return list;
            }
        }

        public void Delete(int id)
        {
            using (var con = _db.GetConnection())
            {
                con.Open();
                con.Execute("DELETE FROM Macros WHERE Id=@Id", new { Id = id });
            }
        }
    }
}
