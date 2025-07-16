using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PREDIOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PREDIOS_DAL";
		private const string TABLA_PREDIOS = "predios";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataSet oDataSet;

		public PREDIOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_predios(string p_chip)
		{
			string sp = "sp_s_predios";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameterString(MySqlCmd, "p_chip", p_chip, "texto");

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}

		public DataSet sp_s_predio_chip(string p_chip)
		{
			string sp = "sp_s_predio_chip";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameterString(MySqlCmd, "p_chip", p_chip, "texto");

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PREDIOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_u_predio(
				string p_chip,
				string p_area_terreno_UAECD,
				string p_area_terreno_folio,
				string p_area_construccion)
		{
			string sp = "sp_u_predio";
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameterString(MySqlCmd, "p_chip", p_chip, "t");
				oDB.MySQLAddParameterString(MySqlCmd, "p_area_terreno_UAECD", p_area_terreno_UAECD, "d");
				oDB.MySQLAddParameterString(MySqlCmd, "p_area_terreno_folio", p_area_terreno_folio, "d");
				oDB.MySQLAddParameterString(MySqlCmd, "p_area_construccion", p_area_construccion, "d");
				oDB.MySQLAddParameterString(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString(), "entero");

				oDB.MySQLAddParameterReturn(MySqlCmd);
				oDB.MySQLExecuteSP(MySqlCmd);
				return MySqlCmd.Parameters["p_Result"].Value.ToString();
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return "ERR12";
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PREDIOS_DAL()
		{
			Dispose(false);
		}
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