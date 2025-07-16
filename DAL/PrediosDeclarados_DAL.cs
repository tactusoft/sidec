using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PREDIOSDECLARADOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PREDIOSDECLARADOS_DAL";
		private const string TABLA_PREDIOSDECLARADOS = "predios_declarados";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public PREDIOSDECLARADOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_predios_dec(string p_opcion, string p_chip_filtro)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_opcion", p_opcion);
				oDB.MySQLAddParameter(MySqlCmd, "p_chip_filtro", p_chip_filtro);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOSDECLARADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_predios_dec_chip(string p_chip)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_chip", p_chip);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOSDECLARADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_predios_dec_carta_terminos(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOSDECLARADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_u_predio_declarado(string p_cod_predio_declarado, string p_numero_caja, string p_numero_carpetas, string p_posicion_carpeta,
						bool p_recibe_carta_terminos, string p_cod_usu_responsable, string p_obs_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_caja", p_numero_caja);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_carpetas", p_numero_carpetas);
				oDB.MySQLAddParameter(MySqlCmd, "p_posicion_carpeta", p_posicion_carpeta);
				oDB.MySQLAddParameter(MySqlCmd, "p_recibe_carta_terminos", p_recibe_carta_terminos);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_predio_declarado", p_obs_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}

		public string sp_i_acompanamiento(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "Error:" + Error.Message;
			}
		}

		public DataSet sp_rpt_estado_predios(string p_cod_declaratoria, string p_cod_usu_responsable, string p_id_estado_predio_declarado, 
						string p_id_estado_predio_declarado2, string p_id_tiempo_cumplimiento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_cod_declaratoria", p_cod_declaratoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_responsable", p_cod_usu_responsable);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_predio_declarado", p_id_estado_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_predio_declarado2", p_id_estado_predio_declarado2);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tiempo_cumplimiento", p_id_tiempo_cumplimiento);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOSDECLARADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}


		public DataSet sp_s_predio_dec_colaboradores(string p_idpredio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
                MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                oDB.MySQLAddParameter(MySqlCmd, "p_idpredio_declarado", p_idpredio_declarado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOSDECLARADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PREDIOSDECLARADOS_DAL()
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		// Free the instance variables of this object.
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				MySqlConn.Dispose();
				MySqlConn = null;

				MySqlDA.Dispose();
				MySqlDA = null;

				oDataSet.Dispose();
				oDataSet = null;
			}
		}
		#endregion
	}
}
