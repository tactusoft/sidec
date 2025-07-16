using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PLANESPCESIONES_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PLANESPCESIONES_DAL";
		private const string TABLA_PLANESPCESIONES = "PlanesPCesiones";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PLANESPCESIONES_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_planesp_cesiones_cod_planp(string p_cod_planp)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PLANESPCESIONES);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_planp_cesion(
			string p_cod_planp,
			string p_unidad_gestion,
			string p_cesion,
			string p_id_tipo_cesion,
			string p_area_cesion,
			string p_porc_ejecutado,
			bool p_es_suelo_en_sitio,
			bool p_es_entregado_DADEP,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_planp", p_cod_planp);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion", p_unidad_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cesion", p_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_cesion", p_id_tipo_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion", p_area_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecutado", p_porc_ejecutado);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_suelo_en_sitio", p_es_suelo_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_entregado_DADEP", p_es_entregado_DADEP);
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
		public string sp_u_planp_cesion(
			string p_au_planp_cesion,
			string p_unidad_gestion,
			string p_cesion,
			string p_id_tipo_cesion,
			string p_area_cesion,
			string p_porc_ejecutado,
			bool p_es_suelo_en_sitio,
			bool p_es_entregado_DADEP,
			string p_observacion
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_cesion", p_au_planp_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_unidad_gestion", p_unidad_gestion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cesion", p_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_cesion", p_id_tipo_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_area_cesion", p_area_cesion);
				oDB.MySQLAddParameter(MySqlCmd, "p_porc_ejecutado", p_porc_ejecutado);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_suelo_en_sitio", p_es_suelo_en_sitio);
				oDB.MySQLAddParameter(MySqlCmd, "p_es_entregado_DADEP", p_es_entregado_DADEP);
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
		public string sp_d_planp_cesion(string p_au_planp_cesion)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_planp_cesion", p_au_planp_cesion);
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

		~PLANESPCESIONES_DAL()
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