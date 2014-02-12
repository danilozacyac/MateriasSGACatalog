using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace MateriasSGA.ConnectionScjn
{
    public class Conexion
    {
        public static SqlConnection GetConexionSql()
        {

            SqlConnection connection = null;

            string bd = "Data Source=CT9BD3;Initial Catalog=IUS;User Id=4cc3s01nf0;Password=Pr0gr4m4d0r3s";
            //string bd = "Data Source=CGCSTHP02;Initial Catalog=IUS;User Id=4cc3s01nf0;Password=Pr0gr4m4d0r3s";

            connection = new SqlConnection(bd);

            return connection;
        }

    }
}
