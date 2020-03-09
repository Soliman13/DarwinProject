using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Data.SQLite;

namespace WpfApp
{
    [Table("Params")]
    public class Params
    {
        [PrimaryKey, Column("Key")]
        public string Key { get; set; }

        [Column("Value")]
        public int Value { get; set; }

        public Params(string key, int value)
        {
            this.Key = key;
            this.Value = value;
        }
        public Params()
        {
        }
    }
}
