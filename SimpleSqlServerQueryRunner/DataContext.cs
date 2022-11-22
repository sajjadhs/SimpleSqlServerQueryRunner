using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Entity.Core.Mapping;
using System.Text.Json.Serialization;

namespace SimpleSqlServerQueryRunner
{
    public class DataContext : DbContext
    {
        private string cn = "";
        public DataContext(string connectionString)
        {
            cn = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings

            if (!cn.ToLower().Contains("trustcervercertificate"))
            {
                cn = "TrustServerCertificate=True;" + cn;
            }

            options.UseSqlServer(cn);
        }

        public bool OpenConnection()
        {
            try
            {
                Database.OpenConnection();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string[] GetTables()
        {
            return Database.SqlQueryRaw<string>("SELECT * FROM sys.Tables").ToArray<string>();
        }

        public string[] GetTableSchema(string tableName)
        {
            return Database.SqlQueryRaw<string>("select CONCAT(COLUMN_NAME, '(', DATA_TYPE,')') from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + tableName + "'").ToArray<string>();
        }

        internal List<string> Run(string sql)
        {
            if (sql.ToLower().StartsWith("select"))
            {
                return Database.SqlQueryRaw<string>(sql + "\nFOR JSON AUTO;").ToList();
            }
            else
            {
                string msg = "";
                try
                {
                    msg = "Affect rows count: " + Database.ExecuteSqlRaw(sql);
                }
                catch (Exception ex)
                {
                    msg = "Error" + ex.Message;
                }
                return new List<string> { { msg } };
            }
        }
    }
}
