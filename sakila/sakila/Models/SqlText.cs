using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace sakila.Models
{
  

    public class SqlText
    {
        public int language_id { get; set; }
        public string name { get; set; }
        public DateTime last_update { get; set; }

        public static readonly string SELECT_QUERY = @"SELECT [language_id]
                                                             ,[name]
                                                             ,[last_update] 
                                                         FROM [sakila].[language]";

        public static string selectQuery = string.Format("SELECT * FROM language");

        public static string deleteQuery = string.Format("DELETE FROM language WHERE language_id= @language_id;");

        public static string insertQuery = string.Format("INSERT INTO language (name) VALUES (@name)");

        public static string updateQuery = string.Format("UPDATE language SET name = @name WHERE language_id = @language_id;");
    }
}
