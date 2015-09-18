using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatalogoSga.Dto;
using MantesisVerIusCommonObjects.Dto;
using ScjnUtilities;

namespace CatalogoSga.Model
{
    public class ClasificacionSgaModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;


        public DataSet GetClasificacion()
        {
            string sql = @"select * from cMateriasSGA Order by consec";
            SqlConnection connection = new SqlConnection(connectionString);
            DataSet ds = new DataSet();

            try
            {
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                da.Fill(ds, "materias");
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }


        //public List<ClasificacionSga> GetClasificacion(int idPadre)
        //{
        //    List<ClasificacionSga> materiasSga = new List<ClasificacionSga>();

        //    string sql = "select * from cMateriasSGA WHERE Padre = @IdPadre Order by consec";
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    SqlCommand cmd;
        //    SqlDataReader reader;

        //    try
        //    {
        //        connection.Open();

        //        cmd = new SqlCommand(sql, connection);
        //        cmd.Parameters.AddWithValue("@IdPadre", idPadre);

        //        reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            ClasificacionSga materia = new ClasificacionSga();

        //            materia.IdClasificacion = Convert.ToInt32(reader["ID"]);
        //            materia.Descripcion = reader["Descripcion"].ToString();
        //            materia.SubClasificaciones = this.GetClasificacion(materia.IdClasificacion);

        //            materiasSga.Add(materia);
        //        }

        //    }
        //    catch (SqlException ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
        //    }
        //    catch (Exception ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    return materiasSga;
        //}

        /// <summary>
        /// Obtiene la estructura completa del catálogo de Materias SGA
        /// </summary>
        /// <param name="padre"></param>
        /// <returns></returns>
        public List<ClasificacionSga> GetClasificacion(int padre)
        {
            List<ClasificacionSga> listaMaterias = new List<ClasificacionSga>();

            SqlConnection connection = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                connection.Open();
                miQry = "Select * FROM cMateriasSGA WHERE padre = @Padre ORDER BY Consec";
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@Padre", padre);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ClasificacionSga materia = new ClasificacionSga(dataReader["Descripcion"].ToString(),
                        GetClasificacion(Convert.ToInt32(dataReader["ID"].ToString())), Convert.ToInt32(dataReader["ID"].ToString()));


                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }

            return listaMaterias;
        }

        /// <summary>
        /// Genera un nuevo registro para la clasificación de Materias SGA
        /// </summary>
        /// <param name="materia">Materia creada</param>
        public void InsertarRegistro(ClasificacionSga materia)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter = null;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Materias");

                dr = dataSet.Tables["Materias"].NewRow();
                dr["Id"] = materia.IdClasificacion;
                dr["Nivel"] = materia.Nivel;
                dr["Padre"] = materia.Padre;
                dr["Descripcion"] = materia.Descripcion;
                dr["SeccionPadre"] = materia.SeccionPadre;
                dr["Historica"] = materia.Historica;
                dr["Consec"] = materia.Consec;
                dr["Hoja"] = materia.Hoja;
                dr["NvlImpresion"] = materia.NvlImpresion;

                dataSet.Tables["Materias"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
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
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }

        }// fin InsertarRegistro

        /// <summary>
        /// Actualiza la información de la Materia SGA que se esta editando
        /// </summary>
        /// <param name="materia">Materia SGA</param>
        public void ActualizaRegistroMaterias(ClasificacionSga materia)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = " + materia.IdClasificacion;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Materia");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Id"] = materia.IdClasificacion;
                dr["Nivel"] = materia.Nivel;
                dr["Padre"] = materia.Padre;
                dr["Descripcion"] = materia.Descripcion;
                dr["SeccionPadre"] = materia.SeccionPadre;
                dr["Historica"] = materia.Historica;
                dr["Consec"] = materia.Consec;
                dr["Hoja"] = materia.Hoja;
                dr["NvlImpresion"] = materia.NvlImpresion;

                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
        }


        /// <summary>
        /// Obtiene el consecutivo de cada una de las materias SGA y le asigna uno nuevo
        /// </summary>
        public void UpdateOrderNumber()
        {
            string sql = @"select * from cMateriasSGA Order by consec";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            int newConsec = 5;

            try
            {
                connection.Open();
                cmd = new SqlCommand(sql, connection);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    this.UpdateConsec(reader, newConsec);

                    newConsec += 5;
                }


            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }



        }

        /// <summary>
        /// Actualiza el consecutivo de las materia SGA
        /// </summary>
        /// <param name="reader">Contiene toda la información que se encuentra en la base de datos de la materia respectiva</param>
        /// <param name="newConsec">Consecutivo que se le asignará a la materia</param>
        private void UpdateConsec(SqlDataReader reader, int newConsec)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM cMateriasSGA WHERE id = " + reader["ID"].ToString();

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Materia");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Id"] = Convert.ToInt64(reader["ID"]);
                dr["Consec"] = newConsec;

                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
        }


        /// <summary>
        /// Obtiene la lista de identificadores de las Materias SGA con la que la tesis fue relacionada
        /// </summary>
        /// <param name="ius">Registro digital de la tesis</param>
        /// <param name="volumen">Volumen del SJF al que pertenece la tesis</param>
        /// <returns></returns>
        public List<int> GetMateriasRelacionadas(long ius, int volumen)
        {
            List<int> listaMaterias = new List<int>();

            SqlConnection connection = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                connection.Open();
                miQry = "Select idMatSGA FROM Tesis_MatSGA WHERE ius = @ius AND Volumen = @Volumen";// +RequestData.Volumen;
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@ius", ius);
                cmd.Parameters.AddWithValue("@Volumen", volumen);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    listaMaterias.Add(Convert.ToInt32(dataReader["idMatSGA"].ToString()));
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }

            return listaMaterias;
        }

        /// <summary>
        /// Devuelve la lista de materias del catálogo SGA asociadas a una tesis en particular, 
        /// se utiliza para el reporte generado después de que se realizan cambios en la tesis
        /// </summary>
        /// <param name="ius">Número de registro digital de la tesis</param>
        /// <returns></returns>
        public List<string> GetMateriasRelacionadas(long ius)
        {
            List<string> listaMaterias = new List<string>();

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                connection.Open();
                miQry = "SELECT T.IdMatSGA,C.descripcion FROM cMateriasSGA C INNER JOIN Tesis_MatSGA T ON T.IdMatSGA = C.ID " +
                        " WHERE IUS = @ius";
                cmd = new SqlCommand(miQry, connection);
                cmd.Parameters.AddWithValue("@ius", ius);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string materia = dataReader["IdMatSGA"].ToString() + "    " + dataReader["Descripcion"].ToString();

                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }

            return listaMaterias;
        }

        /// <summary>
        /// Establece la relación que se esta generando entre una tesis y una materia del catálogo de Materias de la SGA
        /// </summary>
        /// <param name="ius">Registro digital de la tesis</param>
        /// <param name="materias">Listado de materias con las que se relaciona la tesis</param>
        /// <param name="volumen">Volumen al que pertenece latesis</param>
        public void SetRelacionMateriasIus(long ius, List<int> materias, int volumen)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                cmd.CommandText = "DELETE FROM Tesis_MatSGA WHERE ius = " + ius + " AND Volumen = " + volumen;
                cmd.ExecuteNonQuery();

                foreach (int materia in materias)
                {
                    int tomo = Convert.ToInt32(materia.ToString().Substring(0, 3));
                    cmd.CommandText = "INSERT INTO Tesis_MatSGA VALUES(" + ius + "," + materia + "," + tomo + ",'"
                        + DateTime.Now.ToString() + "','Mantesis',0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "'," + volumen + ")";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Bitacora_MateriasSGA VALUES(" + ius + "," + materia + "," + tomo + ",'Mantesis',0,"
                        + volumen + ",0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "',GETDATE())";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
        }

        public List<ClasificacionSga> GetEstructuraNivel(int padre, bool isReadOnly)
        {
            List<ClasificacionSga> listaMaterias = new List<ClasificacionSga>();

            SqlConnection connection = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                connection.Open();
                miQry = "Select * FROM cMateriasSGA WHERE padre = " + padre + " ORDER BY Consec";
                cmd = new SqlCommand(miQry, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    ClasificacionSga materia = new ClasificacionSga(dataReader["Descripcion"].ToString(),
                        GetEstructuraNivel(Convert.ToInt32(dataReader["ID"].ToString()), isReadOnly), Convert.ToInt32(dataReader["ID"].ToString()));

                    materia.IsReadOnly = isReadOnly;

                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ClasificacionSgaModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }

            return listaMaterias;
        }



        

    }
}
