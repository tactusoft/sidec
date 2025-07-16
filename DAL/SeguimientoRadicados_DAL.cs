using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class SeguimientoRadicados_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "SeguimientoRadicados_DAL";
		private const string TABLA_SEGUIMIENTORADICADOS = "SeguimientoRadicados";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public SeguimientoRadicados_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}

		public DataSet sp_s_seguimientoradicados(string p_idseguimiento)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento", p_idseguimiento);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_SEGUIMIENTORADICADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_seguimientoradicados_consultar(string p_idseguimiento_radicado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento_radicado", p_idseguimiento_radicado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_SEGUIMIENTORADICADOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public string sp_i_seguimientoradicados(string p_idseguimiento, string p_identidad, string p_idtiporadicado, string p_radicado,
				string p_fecha, string p_idtramite, string p_otrotramite, string p_idasunto, string p_observaciones_radicado, string p_ruta_archivo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento", p_idseguimiento);
				oDB.MySQLAddParameter(MySqlCmd, "p_identidad", p_identidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtiporadicado", p_idtiporadicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha", p_fecha);

				oDB.MySQLAddParameter(MySqlCmd, "p_idtramite", p_idtramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_otrotramite", p_otrotramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_idasunto", p_idasunto);
				oDB.MySQLAddParameter(MySqlCmd, "p_observaciones_radicado", p_observaciones_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);

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

		public string sp_u_seguimientoradicados(string p_idseguimiento_radicado, string p_identidad, string p_idtiporadicado, string p_radicado,
				string p_fecha, string p_idtramite, string p_otrotramite, string p_idasunto, string p_observaciones_radicado, string p_ruta_archivo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento_radicado", p_idseguimiento_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_identidad", p_identidad);
				oDB.MySQLAddParameter(MySqlCmd, "p_idtiporadicado", p_idtiporadicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha", p_fecha);

				oDB.MySQLAddParameter(MySqlCmd, "p_idtramite", p_idtramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_otrotramite", p_otrotramite);
				oDB.MySQLAddParameter(MySqlCmd, "p_idasunto", p_idasunto);
				oDB.MySQLAddParameter(MySqlCmd, "p_observaciones_radicado", p_observaciones_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_ruta_archivo", p_ruta_archivo);

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

		public string sp_d_seguimientoradicados(string p_idseguimiento_radicado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idseguimiento_radicado", p_idseguimiento_radicado);
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



		#region-----DISPOSE
		// Metodo para el manejo del GC
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~SeguimientoRadicados_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion

	}
}