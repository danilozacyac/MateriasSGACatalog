using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CatalogoSga.Dto;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;

namespace CatalogoSga.Model
{
    public class RelacionesModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString;
        
        /// <summary>
        /// Obtiene las tesis relacionadas a un elemento de la clasificación de materias SGA de un Volumen particular del 
        /// Semanario
        /// </summary>
        /// <param name="idClasificacion">Identificador de la clasificación seleccionada</param>
        /// <param name="volumen">Volumen del Semanario que se esta solicitando</param>
        /// <returns></returns>
        public List<Tesis> GetTesisRelacionadas(int idClasificacion, int volumen)
        {
            List<Tesis> tesisRelacionadas = new List<Tesis>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                string sqlCadena = "SELECT IdMatSGA,T.* FROM Tesis T INNER JOIN Tesis_MatSGA M ON T.IUS = M.IUS " +
                                   "WHERE T.Volumen = @Volumen AND IdMatSGA = @IdMatSGA ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Volumen", volumen);
                cmd.Parameters.AddWithValue("@IdMatSGA", idClasificacion);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String volumenStr = Utils.GetInfoVolumenes(volumen);

                    Tesis tesis = new Tesis();
                    tesis.Ius = Convert.ToInt32(reader["IUS"]);
                    tesis.Rubro = reader["rubro"].ToString();
                    tesis.Texto = reader["texto"].ToString();
                    tesis.Precedentes = reader["Prec"].ToString();
                    Utils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"]));
                    Utils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"]));
                    Utils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"]));
                    tesis.Sala = Convert.ToInt16(reader["sala"]);
                    tesis.Fuente = Convert.ToInt16(reader["fuente"]);
                    tesis.Volumen = volumenStr;
                    tesis.VolumenInt = Convert.ToInt32(reader["Volumen"]);
                    tesis.Tesis = reader["tesis"].ToString();
                    tesis.Pagina = reader["pagina"].ToString();
                    tesis.TatjStr = (Convert.ToInt16(reader["ta_tj"]) == 0) ? "Tesis Aislada" : "Jurisprudencia";
                    tesis.Materia1Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"]));
                    tesis.Materia2Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"]));
                    tesis.Materia3Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"]));
                    tesis.TaTj = Convert.ToInt32(reader["ta_tj"]);
                    tesis.Materia1 = Convert.ToInt16(reader["materia1"]);
                    tesis.Materia2 = Convert.ToInt16(reader["materia2"]);
                    tesis.Materia3 = Convert.ToInt16(reader["materia3"]);
                    tesis.RubroStr = reader["RIndx"].ToString();
                    tesis.IdSubVolumen = Convert.ToInt32(reader["idSubVolumen"]);
                    tesis.IdClasificacionSga = Convert.ToInt64(reader["IdMatSGA"]);

                    tesisRelacionadas.Add(tesis);
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelacionesModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelacionesModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
            return tesisRelacionadas;
        }

        /// <summary>
        /// Obtiene todas las tesis relacionadas a una clasificación específica 
        /// </summary>
        /// <param name="idClasificacion">Identificador de la clasificación seleccionada</param>
        /// <returns></returns>
        public List<Tesis> GetTesisRelacionadas(int idClasificacion)
        {
            List<Tesis> tesisRelacionadas = new List<Tesis>();

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd;
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                string sqlCadena = "SELECT IdMatSGA,T.* FROM Tesis T INNER JOIN Tesis_MatSGA M ON T.IUS = M.IUS " +
                                   "WHERE IdMatSGA = @IdMatSGA  ORDER BY ConsecIndx";

                cmd = new SqlCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@IdMatSGA", idClasificacion);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Tesis tesis = new Tesis();
                    tesis.Ius = Convert.ToInt32(reader["IUS"]);
                    tesis.Rubro = reader["rubro"].ToString();
                    tesis.Texto = reader["texto"].ToString();
                    tesis.Precedentes = reader["Prec"].ToString();
                    Utils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"]));
                    Utils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"]));
                    Utils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"]));
                    tesis.Sala = Convert.ToInt16(reader["sala"]);
                    tesis.Fuente = Convert.ToInt16(reader["fuente"]);
                    tesis.VolumenInt = Convert.ToInt32(reader["Volumen"]);
                    tesis.Volumen = Utils.GetInfoVolumenes(tesis.VolumenInt);
                    tesis.Tesis = reader["tesis"].ToString();
                    tesis.Pagina = reader["pagina"].ToString();
                    tesis.TatjStr = (Convert.ToInt16(reader["ta_tj"]) == 0) ? "Tesis Aislada" : "Jurisprudencia";
                    tesis.Materia1Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"]));
                    tesis.Materia2Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"]));
                    tesis.Materia3Str = Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"]));
                    tesis.TaTj = Convert.ToInt32(reader["ta_tj"]);
                    tesis.Materia1 = Convert.ToInt16(reader["materia1"]);
                    tesis.Materia2 = Convert.ToInt16(reader["materia2"]);
                    tesis.Materia3 = Convert.ToInt16(reader["materia3"]);
                    tesis.RubroStr = reader["RIndx"].ToString();
                    tesis.IdSubVolumen = Convert.ToInt32(reader["idSubVolumen"]);
                    tesis.IdClasificacionSga = Convert.ToInt64(reader["IdMatSGA"]);

                    tesisRelacionadas.Add(tesis);
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelacionesModel", "SgaControl");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,RelacionesModel", "SgaControl");
            }
            finally
            {
                connection.Close();
            }
            return tesisRelacionadas;
        }
    }
}