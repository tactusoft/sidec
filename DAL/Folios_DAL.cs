using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
   public class FOLIOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "FOLIOS_DAL";
        readonly clGlobalVar oVar = new clGlobalVar();
		readonly clDB oDB = new clDB();

		private const string TABLA_FOLIOS = "folios";

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public FOLIOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_folios(string p_year, string p_filtro)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_year", p_year);
				oDB.MySQLAddParameter(MySqlCmd, "p_filtro", p_filtro);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_FOLIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_folio(string p_idfolio)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idfolio", p_idfolio);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_FOLIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_foliolimites()
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_FOLIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_folio(string p_fecha_evento , string p_carpeta, string p_nombre, string p_radicado, string p_fecha_radicado,
								string p_folio_inicial, int p_folio_final, string p_observaciones, string p_idarchivo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_evento", p_fecha_evento);
				oDB.MySQLAddParameter(MySqlCmd, "p_carpeta", p_carpeta);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial", p_folio_inicial);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_final", p_folio_final);
				oDB.MySQLAddParameter(MySqlCmd, "p_observaciones", p_observaciones);
				oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
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
		public string sp_u_folio(string p_idfolio, string p_fecha_evento, string p_carpeta, string p_nombre, string p_radicado, string p_fecha_radicado,
								string p_folio_inicial, int p_folio_final, string p_observaciones, string p_idarchivo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idfolio", p_idfolio);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_evento", p_fecha_evento);
				oDB.MySQLAddParameter(MySqlCmd, "p_carpeta", p_carpeta);
				oDB.MySQLAddParameter(MySqlCmd, "p_nombre", p_nombre);
				oDB.MySQLAddParameter(MySqlCmd, "p_radicado", p_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_radicado", p_fecha_radicado);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_inicial", p_folio_inicial);
				oDB.MySQLAddParameter(MySqlCmd, "p_folio_final", p_folio_final);
				oDB.MySQLAddParameter(MySqlCmd, "p_observaciones", p_observaciones);
				oDB.MySQLAddParameter(MySqlCmd, "p_idarchivo", p_idarchivo);
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
		public string sp_d_folio(string p_idfolio)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_idfolio", p_idfolio);
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

		~FOLIOS_DAL()
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