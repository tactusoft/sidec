using GLOBAL.DB;
using GLOBAL.LOG;
using GLOBAL.UTIL;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class ACTOSADMIN_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "ACTOSADMIN_DAL";
		private const string TABLA_ACTOSADMIN = "actos_administrativos";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clLog oLog = new clLog();
		private readonly clUtil oUtil = new clUtil();
		private readonly clDB oDB = new clDB();

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public ACTOSADMIN_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_actos_administrativos_cod_predio(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_ACTOSADMIN);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_acto_administrativo(
			string p_cod_predio_declarado,
			string p_id_tipo_acto,
			string p_numero_acto,
			string p_fecha_acto,
			string p_id_estado_predio_declarado,
			string p_suspension_meses,
			string p_suspension_dias,
			string p_id_causal_acto,
			string p_fecha_notificacion_acto,
			string p_fecha_ejecutoria_acto,
			string p_fecha_vencimiento_acto,
			string p_obs_acto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_acto", p_id_tipo_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_acto", p_numero_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_acto", p_fecha_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_predio_declarado", p_id_estado_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_suspension_meses", p_suspension_meses);
				oDB.MySQLAddParameter(MySqlCmd, "p_suspension_dias", p_suspension_dias);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_causal_acto", p_id_causal_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notificacion_acto", p_fecha_notificacion_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria_acto", p_fecha_ejecutoria_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_vencimiento_acto", p_fecha_vencimiento_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_acto", p_obs_acto);
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
		public string sp_u_acto_administrativo(
			string p_au_acto_administrativo,
			string p_id_tipo_acto,
			string p_numero_acto,
			string p_fecha_acto,
			string p_id_estado_predio_declarado,
			string p_suspension_meses,
			string p_suspension_dias,
			string p_id_causal_acto,
			string p_fecha_notificacion_acto,
			string p_fecha_ejecutoria_acto,
			string p_fecha_vencimiento_acto,
			string p_obs_acto)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_acto_administrativo", p_au_acto_administrativo);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_tipo_acto", p_id_tipo_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_numero_acto", p_numero_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_acto", p_fecha_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_estado_predio_declarado", p_id_estado_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_suspension_meses", p_suspension_meses);
				oDB.MySQLAddParameter(MySqlCmd, "p_suspension_dias", p_suspension_dias);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_causal_acto", p_id_causal_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_notificacion_acto", p_fecha_notificacion_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_ejecutoria_acto", p_fecha_ejecutoria_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_vencimiento_acto", p_fecha_vencimiento_acto);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_acto", p_obs_acto);
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
		public string sp_d_acto_administrativo(string p_au_acto_administrativo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_acto_administrativo", p_au_acto_administrativo);
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
		public DataSet sp_rpt_actos_administrativos(
			string FecIni,
			string FecFin,
			string IdTipoActo,
			string IdCausal,
			string CHIP)
		{
			oDataSet.Clear();
			oDataSet.Reset();
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;

			try
			{
				MySqlConn.Open();

				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				MySqlCmd.Parameters.Add(new MySqlParameter("p_fecha_acto_ini", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_fecha_acto_ini"].Value = string.IsNullOrEmpty(FecIni) ? null : oUtil.ConvertToFechaDB(FecIni);
				MySqlCmd.Parameters.Add(new MySqlParameter("p_fecha_acto_fin", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_fecha_acto_fin"].Value = string.IsNullOrEmpty(FecFin) ? null : oUtil.ConvertToFechaDB(FecFin);
				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_tipo_acto", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_tipo_acto"].Value = string.IsNullOrEmpty(IdTipoActo) ? null : IdTipoActo;
				MySqlCmd.Parameters.Add(new MySqlParameter("p_id_causal_acto", MySqlDbType.Int16));
				MySqlCmd.Parameters["p_id_causal_acto"].Value = string.IsNullOrEmpty(IdCausal) ? null : IdCausal;
				MySqlCmd.Parameters.Add(new MySqlParameter("p_chip", MySqlDbType.VarChar));
				MySqlCmd.Parameters["p_chip"].Value = string.IsNullOrEmpty(CHIP) ? null : CHIP;

				MySqlDA.SelectCommand = MySqlCmd;
				MySqlDA.Fill(oDataSet, TABLA_ACTOSADMIN);

				MySqlConn.Close();
				return oDataSet;
			}
			catch (Exception Error)
			{
				oLog.RegistrarLogError(Error, _SOURCEPAGE, sp);
				MySqlConn.Close();
				return null;
			}
		}

		#region-----DISPOSE
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true);
		}

		~ACTOSADMIN_DAL()
		{
			// Finalizer calls Dispose(false)
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