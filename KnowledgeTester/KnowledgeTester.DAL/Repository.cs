using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace KnowledgeTester.DAL
{
    public abstract class Repository
    {
        public readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        //public Repository()
        //{

        //    using (SqlConnection connection = new SqlConnection())
        //    {
        //        connection.ConnectionString = _connectionString;
        //        connection.Open();
        //    }
        //}

    }
}
