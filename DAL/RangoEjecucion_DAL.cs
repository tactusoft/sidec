using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using SigesTO;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
	public class RangoEjecucion_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "RangoEjecucion_DAL";
		readonly clGlobalVar oVar = new clGlobalVar();
		readonly clDB oDB = new clDB();

		private const string TABLA_RANGO = "RangoEjecucion";

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public RangoEjecucion_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_rangos_ejecucion(string p_id_categoria, string p_nombre)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_id_categoria", p_id_categoria);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_RANGO);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_rango_ejecucion(string p_idrango)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idrango", p_idrango);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_RANGO);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_u_rango_ejecucion(RangoEjecucionTO r)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idrango_ejecucion", r.IdRangoEjecucionTO.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_dias_limite", r.DiasLimite.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_porcentaje_limite", r.PorcentajeLimite.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_dias_limite_critico", r.DiasLimiteCritico.ToString());
				oDB.MySQLAddParameter(MySqlCmd, "p_porcentaje_limite_critico", r.PorcentajeLimiteCritico.ToString());
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

		~RangoEjecucion_DAL()
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