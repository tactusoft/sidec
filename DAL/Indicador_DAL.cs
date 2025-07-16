using GLOBAL.DB;
using MySql.Data.MySqlClient;
using SigesTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class Indicador_DAL : IDisposable
    {
        private const string _SOURCEPAGE = "INDICADOR_DAL";
        private readonly clDB oDB = new clDB();
        private readonly MySqlConnection MySqlConn;


        public Indicador_DAL()
        {
            string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
            MySqlConn = new MySqlConnection(strConnString);
        }

        public List<IndicadorTO> GetIndicadores(string p_codigo, string p_anio, string p_mes, string p_detalle)
        {
            using (MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString))
            {
                try
                {
                    MySqlConn.Open();
                    List<IndicadorTO> indicadores = new List<IndicadorTO>();

                    using (MySqlCommand MySqlCmd = new MySqlCommand("sp_s_indicador", MySqlConn))
                    {
                        MySqlCmd.CommandType = CommandType.StoredProcedure;

                        oDB.MySQLAddParameter(MySqlCmd, "p_codigo", p_codigo);
                        oDB.MySQLAddParameter(MySqlCmd, "p_anio", p_anio);
                        oDB.MySQLAddParameter(MySqlCmd, "p_mes", p_mes);
                        oDB.MySQLAddParameter(MySqlCmd, "p_detalle", p_detalle);

                        using (MySqlDataAdapter da = new MySqlDataAdapter(MySqlCmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    indicadores.Add(new IndicadorTO
                                    {
                                        Orden = Convert.ToInt32(row["orden"].ToString()),
                                        Orden2 = Convert.ToInt32(row["orden2"].ToString()),
                                        Codigo = row["codigo"].ToString(),
                                        Descripcion1 = row["descripcion1"].ToString(),
                                        Valor1 = Convert.ToDecimal("0" + (row["valor1"] ?? 0).ToString()),
                                        Valor2 = Convert.ToDecimal("0" + (row["valor2"] ?? 0).ToString()),
                                        Valor3 = Convert.ToDecimal("0" + (row["valor3"] ?? 0).ToString()),
                                        Valor4 = Convert.ToDecimal("0" + (row["valor4"] ?? 0).ToString()),
                                        Color = row["color"].ToString()
                                    });
                                }
                            }
                        }
                    }

                    return indicadores;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener los indicadores", ex);
                }
            }
        }
        public List<IndicadorReferenciaTO> GetIndicadores_Referencia()
        {
            using (MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString))
            {
                try
                {
                    MySqlConn.Open();
                    List<IndicadorReferenciaTO> referencias = new List<IndicadorReferenciaTO>();

                    using (MySqlCommand MySqlCmd = new MySqlCommand("sp_s_indicador_referencia", MySqlConn))
                    {
                        MySqlCmd.CommandType = CommandType.StoredProcedure;

                        using (MySqlDataAdapter da = new MySqlDataAdapter(MySqlCmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            if (ds != null && ds.Tables.Count > 0)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    referencias.Add(new IndicadorReferenciaTO{IdReferencia = Convert.ToInt32(row["idreferencia"].ToString()),
                                        Nombre = row["nombre"].ToString(),
                                        NivelDetalle = Convert.ToInt32(row["nivel_detalle"].ToString()),
                                        ConDetalle = (row["con_detalle"].ToString() == "1"),
                                        Descripcion1 = row["complemento1"].ToString(),
                                        Descripcion2 = row["complemento2"].ToString(),
                                        Descripcion3 = row["complemento3"].ToString(),
                                        Descripcion4 = row["complemento4"].ToString(),
                                        Descripcion5 = row["complemento5"].ToString(),
                                        Descripcion6 = row["complemento6"].ToString()
                                    });
                                }
                            }
                        }
                    }

                    return referencias;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener los indicadores", ex);
                }
            }
        }

        public DataSet GetIndicadorDetalle(string p_codigo, string p_anio, string p_mes)
        {
            using (MySqlConnection MySqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString))
            {
                try
                {
                    using (MySqlCommand MySqlCmd = new MySqlCommand("sp_s_indicador_detalle", MySqlConn))
                    {
                        MySqlCmd.CommandType = CommandType.StoredProcedure;

                        oDB.MySQLAddParameter(MySqlCmd, "p_codigo", p_codigo);
                        oDB.MySQLAddParameter(MySqlCmd, "p_anio", p_anio);
                        oDB.MySQLAddParameter(MySqlCmd, "p_mes", p_mes);


                        return oDB.MySQLExecuteSPSelect(MySqlCmd, "DETAIL");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener el detalle de los indicadores", ex);
                }
            }
        }



        public void sp_di_indicadores()
        {
            string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn) { CommandType = CommandType.StoredProcedure };
                oDB.MySQLExecuteSP(MySqlCmd);
            }
            catch (Exception Error)
            {
                oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
            }
        }

        #region-----DISPOSE
        // Metodo para el manejo del GC
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        ~Indicador_DAL()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        // Free the instance variables of this object.
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
        #endregion
    }
}