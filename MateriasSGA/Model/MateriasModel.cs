using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using MateriasSGA.ConnectionScjn;
using MateriasSGA.Dto;
using System.Collections.Generic;

namespace MateriasSGA.Model
{
    public class MateriasModel
    {
        public DataSet GetTodaslasMaterias()
        {
            string sql = @"select * from cMateriasSGA Order by consec";
            SqlConnection conn = Conexion.GetConexionSql();
            DataSet ds = null;

            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                ds = new DataSet();
                da.Fill(ds, "materias");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            finally
            {
                conn.Close();
            }
            return ds; 
        }

        public void InsertarRegistro(Materia materia)
        {
            SqlConnection connectionEpsSql = Conexion.GetConexionSql();
            SqlDataAdapter dataAdapter = null;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionEpsSql);

                dataAdapter.Fill(dataSet, "Materias");

                dr = dataSet.Tables["Materias"].NewRow();
                dr["Id"] = materia.MateriaInt;
                dr["Nivel"] = materia.Nivel;
                dr["Padre"] = materia.Padre;
                dr["Descripcion"] = materia.Descripcion;
                dr["SeccionPadre"] = materia.SeccionPadre;
                dr["Historica"] = materia.Historica;
                dr["Consec"] = materia.Consec;
                dr["Hoja"] = materia.Hoja;
                dr["NvlImpresion"] = materia.NvlImpresion;

                dataSet.Tables["Materias"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionEpsSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO cMateriasSGA(Id,Nivel,Padre,Descripcion,SeccionPadre,Historica,Consec,Hoja,NvlImpresion)" +
                                                        " VALUES(@Id,@Nivel,@Padre,@Descripcion,@SeccionPadre,@Historica,@Consec,@Hoja,@NvlImpresion)";

                dataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.BigInt, 0, "Id");
                dataAdapter.InsertCommand.Parameters.Add("@Nivel", SqlDbType.Int, 0, "Nivel");
                dataAdapter.InsertCommand.Parameters.Add("@Padre", SqlDbType.Int, 0, "Padre");
                dataAdapter.InsertCommand.Parameters.Add("@Descripcion", SqlDbType.VarChar, 0, "Descripcion");
                dataAdapter.InsertCommand.Parameters.Add("@SeccionPadre", SqlDbType.Int, 0, "SeccionPadre");
                dataAdapter.InsertCommand.Parameters.Add("@Historica", SqlDbType.Int, 0, "Historica");
                dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                dataAdapter.InsertCommand.Parameters.Add("@Hoja", SqlDbType.Int, 0, "Hoja");
                dataAdapter.InsertCommand.Parameters.Add("@NvlImpresion", SqlDbType.Int, 0, "NvlImpresion");


                dataAdapter.Update(dataSet, "Materias");
            }
            catch (SqlException) { }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsSql.Close();
            }
            
        }// fin InsertarRegistro

        public void ActualizaRegistroMaterias(Materia materia)
        {
            SqlConnection sqlConne = Conexion.GetConexionSql();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = " + materia.MateriaInt;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, sqlConne);

                dataAdapter.Fill(dataSet, "Materia");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Id"] = materia.MateriaInt;
                dr["Nivel"] = materia.Nivel;
                dr["Padre"] = materia.Padre;
                dr["Descripcion"] = materia.Descripcion;
                dr["SeccionPadre"] = materia.SeccionPadre;
                dr["Historica"] = materia.Historica;
                dr["Consec"] = materia.Consec;
                dr["Hoja"] = materia.Hoja;
                dr["NvlImpresion"] = materia.NvlImpresion;

                dr.EndEdit();

                dataAdapter.UpdateCommand = sqlConne.CreateCommand();

                string sSql = "UPDATE cMateriasSGA " +
                              "SET Nivel = @Nivel, " +
                              " Padre = @Padre, Descripcion = @Descripcion, SeccionPadre = @SeccionPadre, Historica = @Historica," +
                              " Consec = @Consec, Hoja = @Hoja,NvlImpresion = @NvlImpresion" +
                              " WHERE Id = @Id";

                dataAdapter.UpdateCommand.CommandText = sSql;

                dataAdapter.UpdateCommand.Parameters.Add("@Nivel", SqlDbType.Int, 0, "Nivel");
                dataAdapter.UpdateCommand.Parameters.Add("@Padre", SqlDbType.Int, 0, "Padre");
                dataAdapter.UpdateCommand.Parameters.Add("@Descripcion", SqlDbType.VarChar, 0, "Descripcion");
                dataAdapter.UpdateCommand.Parameters.Add("@SeccionPadre", SqlDbType.Int, 0, "SeccionPadre");
                dataAdapter.UpdateCommand.Parameters.Add("@Historica", SqlDbType.Int, 0, "Historica");
                dataAdapter.UpdateCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                dataAdapter.UpdateCommand.Parameters.Add("@Hoja", SqlDbType.Int, 0, "Hoja");
                dataAdapter.UpdateCommand.Parameters.Add("@NvlImpresion", SqlDbType.Int, 0, "NvlImpresion");
                dataAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.BigInt, 0, "Id");


                dataAdapter.Update(dataSet, "Materia");
                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }



        public void UpdateOrderNumber()
        {
            string sql = @"select * from cMateriasSGA Order by consec";
            SqlConnection conn = Conexion.GetConexionSql();
            SqlCommand cmd;
            SqlDataReader reader;

            Dictionary<long, int> numConsec = new Dictionary<long, int>();
            int newConsec = 5;

            try
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    this.UpdateConsec(reader,newConsec);

                    newConsec += 5;
                }

                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
            finally
            {
                conn.Close();
            }
            


        }


        private void UpdateConsec(SqlDataReader reader,int newConsec)
        {
            SqlConnection sqlConne = Conexion.GetConexionSql();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = " + reader["ID"].ToString();

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, sqlConne);

                dataAdapter.Fill(dataSet, "Materia");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Id"] = Convert.ToInt64(reader["ID"]);
                dr["Consec"] = newConsec;

                dr.EndEdit();

                dataAdapter.UpdateCommand = sqlConne.CreateCommand();

                string sSql = "UPDATE cMateriasSGA " +
                              "SET Consec = @Consec " +
                              " WHERE Id = @Id";

                dataAdapter.UpdateCommand.CommandText = sSql;

                dataAdapter.UpdateCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                dataAdapter.UpdateCommand.Parameters.Add("@Id", SqlDbType.BigInt, 0, "Id");


                dataAdapter.Update(dataSet, "Materia");
                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException sql)
            {
                MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }

        
    }
}