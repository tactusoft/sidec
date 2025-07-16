using GLOBAL.DB;
using GLOBAL.VAR;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace GLOBAL.DAL
{
    public class PRESTAMOS_DAL : IDisposable
	{
		private const string _SOURCEPAGE = "PRESTAMOS_DAL";

		private readonly clGlobalVar oVar = new clGlobalVar();
		private readonly clDB oDB = new clDB();

		private const string TABLA_PRESTAMOS = "prestamos";

		private MySqlConnection MySqlConn;
		private MySqlDataAdapter MySqlDA;
		private DataTable oDataTable;
		private DataSet oDataSet;

		public PRESTAMOS_DAL()
		{
			string strConnString = ConfigurationManager.ConnectionStrings["sidecConn"].ConnectionString;
			MySqlConn = new MySqlConnection(strConnString);
			MySqlDA = new MySqlDataAdapter();
			oDataTable = new DataTable();
			oDataSet = new DataSet();
		}
		public DataSet sp_s_prestamos_cod_predio(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PRESTAMOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_s_prestamos_cod_predio_ultimo(string p_cod_predio_declarado)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PRESTAMOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public DataSet sp_rpt_prestamos_abiertos()
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				return oDB.MySQLExecuteSPSelect(MySqlCmd, TABLA_PRESTAMOS);
			}
			catch (Exception Error)
			{
				oDB.MySQLSPError(Error, _SOURCEPAGE, sp);
				return null;
			}
		}
		public string sp_i_prestamo(
			string p_cod_predio_declarado,
			string p_id_area_solicita_prestamo,
			string p_cod_usu_solicita_prestamo,
			string p_memorando_interno,
			string p_fecha_entrega_prestamo,
			string p_cod_usu_entrega_prestamo,
			string p_folios_prestamo,
			string p_fecha_devolucion_prestamo,
			string p_cod_usu_recibe_prestamo,
			string p_obs_prestamo
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predio_declarado", p_cod_predio_declarado);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_area_solicita_prestamo", p_id_area_solicita_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_solicita_prestamo", p_cod_usu_solicita_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_memorando_interno", p_memorando_interno);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega_prestamo", p_fecha_entrega_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_entrega_prestamo", p_cod_usu_entrega_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_folios_prestamo", p_folios_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion_prestamo", p_fecha_devolucion_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_recibe_prestamo", p_cod_usu_recibe_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_prestamo", p_obs_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
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
		public string sp_i_prestamo_lote(
			string p_opcion,
			string p_cod_predios_declarados,
			string p_id_area_solicita_prestamo,
			string p_cod_usu_solicita_prestamo,
			string p_memorando_interno,
			string p_fecha_entrega_prestamo,
			string p_cod_usu_entrega_prestamo,
			string p_fecha_devolucion_prestamo,
			string p_cod_usu_recibe_prestamo,
			string p_obs_prestamo)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_opcion", p_opcion);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_predios_declarados", p_cod_predios_declarados);
				oDB.MySQLAddParameter(MySqlCmd, "p_id_area_solicita_prestamo", p_id_area_solicita_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_solicita_prestamo", p_cod_usu_solicita_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_memorando_interno", p_memorando_interno);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_entrega_prestamo", p_fecha_entrega_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_entrega_prestamo", p_cod_usu_entrega_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion_prestamo", p_fecha_devolucion_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_recibe_prestamo", p_cod_usu_recibe_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_prestamo", p_obs_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
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
		public string sp_u_prestamo(
			string p_au_prestamo,
			string p_memorando_interno,
			string p_fecha_devolucion_prestamo,
			string p_cod_usu_recibe_prestamo,
			string p_obs_prestamo
		)
		{
			string sp = System.Reflection.MethodBase.GetCurrentMethod().Name;
			try
			{
				MySqlCommand MySqlCmd = new MySqlCommand(sp, MySqlConn);
				MySqlCmd.CommandType = CommandType.StoredProcedure;

				oDB.MySQLAddParameter(MySqlCmd, "p_au_prestamo", p_au_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_memorando_interno", p_memorando_interno);
				oDB.MySQLAddParameter(MySqlCmd, "p_fecha_devolucion_prestamo", p_fecha_devolucion_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu_recibe_prestamo", p_cod_usu_recibe_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_obs_prestamo", p_obs_prestamo);
				oDB.MySQLAddParameter(MySqlCmd, "p_cod_usu", oVar.prUserCod.ToString());
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

		~PRESTAMOS_DAL()
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