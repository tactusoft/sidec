using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PLANESPACTOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PLANESPACTOS_DAL";
		private const string TABLA_PLANESPACTOS = "PlanesPActos";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PLANESPACTOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_planesp_actos_cod_planp(string p_cod_planp)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESPACTOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_planp_acto(string p_cod_planp, string p_id_tipo_acto, string p_numero_acto, string p_fecha_acto,
					string p_vigencia, string p_observacion)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_acto", p_id_tipo_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_acto", p_numero_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_acto", p_fecha_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_vigencia", p_vigencia);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
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
		public string sp_u_planp_acto(string p_au_planp_acto, string p_id_tipo_acto, string p_numero_acto, string p_fecha_acto,
					string p_vigencia, string p_observacion, bool p_is_file )
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_acto", p_au_planp_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_acto", p_id_tipo_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_acto", p_numero_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_acto", p_fecha_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_vigencia", p_vigencia);
				oDB.MySQLAddParameter(MySqlCmd, "p_observacion", p_observacion);
				oDB.MySQLAddParameter(MySqlCmd, "p_is_file", p_is_file);
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
		public string sp_d_planp_acto(string p_au_planp_acto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_acto", p_au_planp_acto);
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
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~PLANESPACTOS_DAL()
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

				oDataTable.Dispose();
				oDataTable = null;
			}
		}
		#endregion
	}
}